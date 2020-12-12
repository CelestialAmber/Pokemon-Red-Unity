using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class PokemonPicImporter : MonoBehaviour
{
    public string picFilePath;
    int currentBit, imageDataOffset;
    //To load Missigno's sprite/other glitch Pokemon sprites, this would have to be changed back to being a single big array
    //and be extended as needed
    byte[] buffer1, buffer2; //temporary storage locations as the sprite's bitplanes are decompressed
    byte[] combinedSpriteData; //final array for the combined 2bpp sprite data
    byte[] imageData;
    int totalBytes;
    int tileWidth, tileHeight, width, height;
    int destinationBuffer;
    int bitplaneX, bitplaneY;
    int destByteIndex, destBitIndex;
    public bool convertDirectory;

    public void LoadPicFile(){
        //int bank = 0;
        //int romOffset = bank * 16384 + bankoffset;
        //byte[] data = File.ReadAllBytes(Application.dataPath + picFilePath).Skip(romOffset).ToArray();
        if(convertDirectory){
        string dir = Application.dataPath + "/Pokemon Pics/";
        string[] files;
        try
        {
            files = Directory.EnumerateFiles(dir,"*.pic",SearchOption.AllDirectories).ToArray();
        }
        catch (DirectoryNotFoundException)
        {
            Debug.Log("Directory not found.");
            return;
        }
        foreach (string file in files)
        {
            Debug.Log("Decompressing " + file);
            DecompressPicFile(file);
        }
        Debug.Log("Finished decompressing all .pic files in directory.");
        }else{
            string path = Application.dataPath + "/" + picFilePath;
            Debug.Log("Decompressing " + Path.GetFileName(path));
            DecompressPicFile(path);
        }
    }


    /*Compression format notes:
    Data format:
    Width/Height (1st byte, in tiles)
    Primary buffer (1 bit): 0 for Buffer 2, 1 for Buffer 3
    1st bitplane compressed data
    Encoding mode (1-2 bits)
    2nd bitplane compressed data

    The compressed data starts with a bit indicating whether the compressed data
    starts with a RLE or regular data group: 0 for rle, 1 for data.
    The end is reached when enough bytes according to the image size are read (i.e. maximum is 7*7 = 49 bytes)
    RLE group format (groups of zeros):
    The number of pairs of zeros to add is stored as two numbers of n bits.
    The first represents the size of the second number in bits directly after it
    (e.g. 11110 = 5 bits)
    number of zeros = 2*(n + 2nd number + 2)
    Regular data format: for each pair of bits, copy them directly to the data array
    unless they're both zero

    Encoding methods:
    Delta encoding: bits are encoded as the difference between them (1 if different, 0 if same)
    example: 10111001 = 11100101
    The bits are encoded with 0 as the starting number (not included with the other bits)

    Xor: both bitplanes are xored together, and replaces the 2nd bitplane

    Mode 0: Delta encode both bitplanes
    Mode 1: BP1 = BP0 xor BP1, delta encode BP0
    Mode 2: BP1 = BPO xor BP1, delta encode both bitplanes

    For each mode, the order of the bitplanes can vary (6 modes total, bit after first width/height byte) 
    */
    public void DecompressPicFile(string path){
        byte[] data = File.ReadAllBytes(path);
        imageData = data;
        imageDataOffset = 0;
        currentBit = 7;
        buffer1 = new byte[392];
        buffer2 = new byte[392];
        combinedSpriteData = new byte[784];
        
        byte sizeByte = imageData[imageDataOffset];
        imageDataOffset++;
        tileWidth = sizeByte >> 4; //first 4 bits
        tileHeight = sizeByte & 0x0F; //lower 4 bits
        width = tileWidth * 8;
        height = tileHeight * 8;
        //Debug.Log("Width: " + tileWidth + ", Height: " + tileHeight);
        int primaryBuffer = GetNextBit();
        //Debug.Log("Primary Buffer: " + primaryBuffer);
        destinationBuffer = primaryBuffer;  

        /*
        Decompress the first bitplane into one of the buffers
        depending on the read value
        */
        DecompressBitplane();
        int mode = GetNextBit(); //the bits for the mode are stored backwards (little endian)?
        if(mode == 1) mode = GetNextBit() + 1; //if the first bit is 1, read the second bit (mode 1/2)
        //Debug.Log("Encoding Mode: " + mode);
        destinationBuffer = 1 - destinationBuffer; //change to the other buffer
        //Decompress the second bitplane into the other buffer
        DecompressBitplane();

        if(mode == 0){
            buffer2 = DeltaDecode(buffer2);
            buffer1 = DeltaDecode(buffer1);
        }else if(mode == 1){
            if(primaryBuffer == 0)buffer1 = DeltaDecode(buffer1);
            else buffer2 = DeltaDecode(buffer2);
            XorBuffers();
        }else{
            buffer2 = DeltaDecode(buffer2);
            buffer1 = DeltaDecode(buffer1);
            XorBuffers();
        }

        CombineBitplanes();

        /*
        Rearrange the sprite data in the correct order unless
        it's the same size as the bounding box (56x56)
        */
        if(tileWidth < 7 || tileHeight < 7) CenterSprite();
        
        ConvertSpriteDataToTexture(path); //save the sprite to png
    }

    public void DecompressBitplane(){
        int groupType = GetNextBit();
        totalBytes = 0;
        bitplaneX = 0;
        bitplaneY = 0;
        destBitIndex = 0;
        destByteIndex = 0;

        while(totalBytes < tileWidth * height){
            if(groupType == 0){ //check if the first data group is an RLE or normal data group
                //RLE group
                int bitsInNumbers = 0; //number of bits in 1st/2nd numbers
                /*
                Value of the highest bit is the 1st number, and also tells how many bits the
                2nd number has, which has the rest of the number other than the highest power
                of 2 in the number
                */
                int mult = 0, lowBits = 0;
                int bitsToAdd = 0;

                int nextBit = GetNextBit();        
                mult = (mult << 1) | nextBit;
                bitsInNumbers++;

                while((mult & 1) == 1){
                    nextBit = GetNextBit();
                    mult = (mult << 1) | nextBit;
                    bitsInNumbers++;
                }

                for(int i = 0; i < bitsInNumbers; i++){
                    nextBit = GetNextBit();
                    lowBits = (lowBits << 1) | nextBit;
                }

                bitsToAdd = lowBits + mult + 1; //calculate the actual number of zeros to add from the stored values
                
                for(int i = 0; i < bitsToAdd; i++){ //add bitsToAdd zeros to the decompressed data
                    WriteBitPair(0, 0);
                    if(totalBytes == tileWidth * height) break;
                }
                groupType = 1;
            }
            else{
                //Data group
                int bit1 = GetNextBit();
                int bit2 = GetNextBit();
                while(bit1 != 0 || bit2 != 0){ //keep going until the current pair is 00
                    WriteBitPair(bit1, bit2);
                    if(totalBytes == tileWidth * height) break;
                    bit1 = GetNextBit();
                    bit2 = GetNextBit();
                }  
                groupType = 0;       
            }
        }
    }

    public void CombineBitplanes(){
        for(int i = 0; i < 392; i++){
            combinedSpriteData[2*i] = buffer1[i];
            combinedSpriteData[2*i + 1] = buffer2[i];
        }
    }
    
    //Fix the alignment of the tiles in the sprite's data so that it's arranged properly
    //This directly causes Missingo's L-block shape
    //starting x position = floor((7-w)/2 + 1/2), y position (from top) = 7-h
    public void CenterSprite(){
        int startXPos = Mathf.FloorToInt((float)(7-tileWidth)/2f + 0.5f);
        int startYPos = 7 - tileHeight;
        byte[] newArray = new byte[784];

        for(int x = 0; x < tileWidth; x++){
            for(int y = 0; y < tileHeight; y++){
                Array.Copy(combinedSpriteData, 16*(x*tileHeight + y), newArray, 16*(7*(x + startXPos) + y + startYPos), 16);
            }
        }  
        combinedSpriteData = newArray;
    }

    public byte[] DeltaDecode(byte[] data){
        byte[] decodedData = new byte[392];
        int lastBit = 0;

        for(int y = 0; y < height; y++){
            for(int x = 0; x < width; x++){
                int index = Mathf.FloorToInt((float)x/8f) * height + y;
                int currentBit = (data[index] >> (7-(x%8))) & 1;

                if(currentBit == 1){
                    lastBit = 1 - lastBit;
                    decodedData[index] |= (byte)(lastBit << (7-(x%8)));
                }else{
                    decodedData[index] |= (byte)(lastBit << (7-(x%8)));
                }
            }
            lastBit = 0; //the last bit resets every row
        }
        return decodedData;
    }

    public void XorBuffers(){
        byte[] newArray = new byte[392];
        
        for(int i = 0; i < 392; i++){
            newArray[i] = (byte)(buffer1[i] ^ buffer2[i]);
        }
 
        if(destinationBuffer == 0) buffer1 = newArray;
        else buffer2 = newArray;
    }

    //The graphics in the initial compressed form are stored in 2 bit pairs starting from
    //top left, going down and right
    public void WriteBitPair(int bit1, int bit2){
        byte[] dest = destinationBuffer == 0 ? buffer1 : buffer2;

        if (destByteIndex < 392){
            dest[destByteIndex] |= (byte)((bit1 << (7 - destBitIndex)) | (bit2 << (6 - destBitIndex)));
        }

        bitplaneY++;
        if (bitplaneY >= height){
            bitplaneY = 0;
            bitplaneX += 2;
        }
        if(bitplaneY % 4 == 0) totalBytes++;
        destBitIndex = bitplaneX % 8;
        destByteIndex = Mathf.FloorToInt((float)bitplaneX/8f) * height + bitplaneY;
    }

    public int GetNextBit(){
        int bit = (imageData[imageDataOffset] >> currentBit) & 1;
        currentBit--;
        if(currentBit == -1){
            currentBit = 7;
            imageDataOffset++; //advance to the next byte
        }
        return bit;
    }

    public Color[] palette = {Color.white, new Color(0.66f,0.66f,0.66f), new Color(0.33f,0.33f,0.33f), Color.black};

    public void ConvertSpriteDataToTexture(string path){
        //convert the gameboy sprite into a texture, then save it as png
        int imageWidth = 56, imageHeight = 56;
        Texture2D image = new Texture2D(imageWidth,imageHeight);
        int xPos = 0,yPos = 0;
        int tileIndex = 0;
        for (int i = 0; i < imageWidth * imageHeight; i += 16)
        {                  
            int highBit = 0, lowBit = 0;
            xPos = (tileIndex / (imageHeight / 8));
            yPos = (tileIndex % (imageHeight / 8));
            for (int x = 0; x < 8; x++){
                for (int y = 0; y < 8; y++){
                    if (i + 2 * x + 1 >= combinedSpriteData.Length) break;
                    Color color;
                    highBit = (combinedSpriteData[i + 2 * y + 1] >> (7 - x)) & 1;
                    lowBit = (combinedSpriteData[i + 2 * y] >> (7 - x)) & 1;
                    int value = (highBit << 1) | lowBit;
                    color = palette[value];
                    image.SetPixel(x + (xPos * 8), imageHeight - 1 - (y + (yPos * 8)), color);
                }
            }                
            tileIndex++;
        }
        string outputFilePath = path.Replace(".pic",".png");
        File.WriteAllBytes(outputFilePath, image.EncodeToPNG());  
    }
}