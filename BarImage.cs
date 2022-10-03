

namespace ImageZap
{
    // based on information found on wikipeda - https://en.wikipedia.org/wiki/BMP_file_format
    // light weight graphic class to create 2d barcodes only. 
    // (C) 2022 Doug Barger (zapdbf)
    public class BarImage
    {
        private byte[] bImageRaw; // image in a byte array
        private List<byte> bHeader=new List<byte>();
        private byte[] BM = { 0x42, 0x4D }; // fixed DIB header type fixed to windows type
        private const byte pBlack = 0x0; //  black pixel all we need is black an white 
        private const byte pWhite = 0xff; // white pixel
       
        private Int32 width;
        private Int32 height;   
public BarImage(Int32 width,Int32 height)
        {
            // since this is for a baccode only we will create a white block as a canvis for the barcode
           
            this.width = width;
            this.height = height;

            Makeheader(width,height);

            // build blank image data
            Int32 size = width * height*4;
            bImageRaw = new byte[size];
            for (int x = 0; x < size; x++)
            {
                bImageRaw[x] = pWhite;
            }




        }
public void MakeBar(Int32 x,Int32 y,Int32 width,Int32 height)

        {
           
            // muted this out for now, diagnosing general image creation 
            // failure
            return;
            // each bar is 4 bytes long r,g,b,cm
            // channel mask fixed at 255 already set
            Int32 yjump = width * 4;
            Int32 xjump = 4;
            Int32 Size = width * height * 4;
            Int32 xl;
            Int32 Xstart;
            Int32 Xstarti;
            Int32 wl;
            Int32 lastBarLoc = (yjump * height) + (xjump * x); // this should point to the last black pixel on the top
            for (int yl= y * yjump; yl< lastBarLoc;yl +=yjump)
            {
                Xstart = yl + (x * xjump);
                Xstarti = Xstart;

                for (wl = 0; wl < width; wl++)
                {
                    for (xl = Xstarti; xl < Xstart + 3; xl++) // make rgb =0 
                    {
                        bImageRaw[xl] = 0x0;
                    }
                    Xstarti++; // skip over channel mask 
                }
                
                
                
                Xstart += yjump; // skip to next line because of Xstart offset should be lined up
                                 // with next line bar location









            }






        }
public byte[] ImageArray
        {get
            {
                List<byte> oout = new List<byte>();
                oout.AddRange(bHeader);
                oout.AddRange(bImageRaw); 
                byte[] b = oout.ToArray();
                return b;

            }



        }
private void Makeheader(Int32 width,Int32 height)
        {
          
            // since this is in memory starting with the DIB, BMP header not needed? in memory
            // does not have may options not sutable for anything other than barcodes
            Byte[] b;
            // LSB first byte

            // BMP header
            bHeader.AddRange(BM); //bm bmp type indicator 
            Int32Converter totalSize = (width *height*4) + 96 +14;
            bHeader.AddRange(totalSize.ByteArray); // file size
            b = new byte[] { 0x0, 0x0, 0x0, 0x0 };
            bHeader.AddRange(b); // unused app specific
            Int32Converter BmpStart = (96 + 14 + 1); // offset , start of bmp data
            bHeader.AddRange(BmpStart.ByteArray);


            // DIB HEADER
            b = new byte[] { 0x60, 0x0, 0x0, 0x0 };
            bHeader.AddRange(b);  // Number of bytes in DIB header
            Int32Converter Width = width;
            Int32Converter Height = height;
            bHeader.AddRange(Width.ByteArray); // header width
            bHeader.AddRange(Height.ByteArray); // header height
            b = new byte[] { 0x1, 0x0 }; 
            bHeader.AddRange(b); // Color planes used fixed at 1
            b = new byte[] { 0x20, 0x0 };
            bHeader.AddRange(b); // Bits per pixel fixed at 32 (ARGB32)
            b = new byte[] {0x3,0x0,0x0,0x0 };
            bHeader.AddRange(b); // BI_BITFIELS, = No pixel array compression used = fixed
            Int32Converter Size = width * height * 4;
            bHeader.AddRange(Size.ByteArray); // total size of picure in bytes
            b = new byte[] { 0x13, 0x0b, 0x0, 0x0 };
            bHeader.AddRange(b); // horizontal fixing 72 dpi print resolution in pixels/meter is actual refreance
            bHeader.AddRange(b); // vertical 72 print resolution
            b = new byte[] { 0x0, 0x0, 0x0, 0x0 };
            bHeader.AddRange(b); // number of colors in the palette (not palited)
            bHeader.AddRange(b); // important colors 0 = all colors are important
             b=new byte[] { 0x0,0x0,0xff,0x0};
            bHeader.AddRange(b); // Red color channel mask
            b = new byte[] { 0x0, 0xff, 0x0, 0x0 };
            bHeader.AddRange(b); // green color mask
            b = new byte[] { 0xff, 0x0, 0x0, 0x0 };
            bHeader.AddRange(b); // blue color mask
            b = new byte[] { 0x0, 0x0, 0x0, 0xff };
            bHeader.AddRange(b); // alpha channel bit mask
            b = new byte[] { 0x20, 0x6e, 0x69, 0x57 };
            bHeader.AddRange(b); // LCS_WINDOWS_COLOR_SPACE
                                // unused bits for this bmp type reserved for
                                // CIEXYZTRIPLE  (24) Color Space endpoints (not used but must have this space)
                                // red gama (4) ,green gama(4),Blue gama (4)
                                //      total 36 bytes
            for (int x=0; x < 36;x++)
                {
                bHeader.Add(0x0);
            }
        
         // header must end in multipls of 4 in order for the pixal data to
         // be seen correctly, also we don't have any gaps so this is a packed DIB
        }


    }
}