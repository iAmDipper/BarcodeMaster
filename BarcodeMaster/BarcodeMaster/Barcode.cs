using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.IO;

namespace BarcodeMaster
{
    public static class Barcode
    {
        private static string GetEncodedPattern(string text)
        {
            /*
                Code Pattern:
                2 = Thick Line
                1 = Thin Line
                0 = While Line
             */

            //Barcode starts and ends with the separator so the scanner can read it
            string separator = "101221";

            string encodedText = "";

            encodedText += separator;

            //Write the code pattern into encodedText foreach character
            foreach (var c in text.ToUpper())
            {
                switch (c)
                {
                    case '1':
                        encodedText += "210112";
                        break;
                    case '2':
                        encodedText += "120112";
                        break;
                    case '3':
                        encodedText += "220111";
                        break;
                    case '4':
                        encodedText += "110212";
                        break;
                    case '5':
                        encodedText += "210211";
                        break;
                    case '6':
                        encodedText += "120211";
                        break;
                    case '7':
                        encodedText += "110122";
                        break;
                    case '8':
                        encodedText += "210121";
                        break;
                    case '9':
                        encodedText += "120121";
                        break;
                    case '0':
                        encodedText += "110221";
                        break;
                    case 'A':
                        encodedText += "211012";
                        break;
                    case 'B':
                        encodedText += "121012";
                        break;
                    case 'C':
                        encodedText += "221011";
                        break;
                    case 'D':
                        encodedText += "112012";
                        break;
                    case 'E':
                        encodedText += "212011";
                        break;
                    case 'F':
                        encodedText += "122011";
                        break;
                    case 'G':
                        encodedText += "111022";
                        break;
                    case 'H':
                        encodedText += "211021";
                        break;
                    case 'I':
                        encodedText += "121021";
                        break;
                    case 'J':
                        encodedText += "112021";
                        break;
                    case 'K':
                        encodedText += "211102";
                        break;
                    case 'L':
                        encodedText += "121102";
                        break;
                    case 'M':
                        encodedText += "221101";
                        break;
                    case 'N':
                        encodedText += "112102";
                        break;
                    case 'O':
                        encodedText += "212101";
                        break;
                    case 'P':
                        encodedText += "122101";
                        break;
                    case 'Q':
                        encodedText += "111202";
                        break;
                    case 'R':
                        encodedText += "211201";
                        break;
                    case 'S':
                        encodedText += "121201";
                        break;
                    case 'T':
                        encodedText += "112201";
                        break;
                    case 'U':
                        encodedText += "201112";
                        break;
                    case 'V':
                        encodedText += "102112";
                        break;
                    case 'W':
                        encodedText += "202111";
                        break;
                    case 'X':
                        encodedText += "101212";
                        break;
                    case 'Y':
                        encodedText += "201211";
                        break;
                    case 'Z':
                        encodedText += "102211";
                        break;
                    case '-':
                        encodedText += "101122";
                        break;
                    case '.':
                        encodedText += "201121";
                        break;
                    case ' ':
                        encodedText += "102121";
                        break;
                    case '$':
                        encodedText += "10101011";
                        break;
                    case '/':
                        encodedText += "10101101";
                        break;
                    case '+':
                        encodedText += "10110101";
                        break;
                    case '%':
                        encodedText += "11010101";
                        break;
                    default:
                        encodedText += "102121";
                        break;
                }
            }

            encodedText += separator;

            return encodedText;
        }

        public static Bitmap CreateBarcode(string Text, bool addText = false)
        {
            //Get Barcode as numbers from text
            string s = GetEncodedPattern(Text);

            //the space between each line
            int space = 5;

            //calculating the width depending on how large the code is
            int height = 130;
            int width = 30;

            //The Barcode lines are drawn by one pencil each

            //Default: new Pen(Color.Black, 2);
            Pen thinPen = new Pen(Color.Black, 2);
            //Default: new Pen(Color.Black, 4);
            Pen thickPen = new Pen(Color.Black, 4);
            //Default: new Pen(Color.White, 4);
            Pen whitePen = new Pen(Color.White, 4);

            //Additional Height if Text is added below Barcode
            int additionalHeight = 0;

            if (addText)
            {
                additionalHeight = 20;
                height += additionalHeight;
            }

            foreach (char character in s)
            {
                //Calculate the needed width for the Barcode
                switch (character)
                {
                    case '0':
                        width += space - (int)whitePen.Width + (int)whitePen.Width;
                        break;
                    case '1':
                        width += space - (int)thinPen.Width + (int)thinPen.Width;
                        break;
                    case '2':
                        width += space - (int)thickPen.Width + (int)thickPen.Width;
                        break;
                    default:
                        break;
                }
            }

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                //p1 and p2 start at 15 pixels to guarantee the quiet zone for the code
                Point p1 = new Point(15, 0);
                Point p2 = new Point(15, height - additionalHeight - 30);

                RectangleF rect = new RectangleF(0, 0, width, height);

                //White Background
                g.FillRectangle(Brushes.White, rect);

                //Draw the Barcode for each character in the encoded string
                foreach (char character in s)
                {
                    switch (character)
                    {
                        case '0':
                            g.DrawLine(whitePen, p1, p2);
                            p1.X += space;
                            p2.X += space;
                            break;
                        case '1':
                            g.DrawLine(thinPen, p1, p2);
                            p1.X += space;
                            p2.X += space;
                            break;
                        case '2':
                            g.DrawLine(thickPen, p1, p2);
                            p1.X += space;
                            p2.X += space;
                            break;
                        default:
                            break;
                    }/**/
                }

                //Add Text Below Barcode to see what the Barcode reads
                if (addText)
                {
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    g.DrawString(Text, new Font("Calibri", 15), Brushes.Black, new RectangleF(0, 110, width, additionalHeight), sf);
                }

                g.DrawImage(bitmap, new PointF(0, 0));
            }

            return bitmap;
        }
    }
}
