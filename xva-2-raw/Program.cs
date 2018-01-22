using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;


namespace xva_2_raw
{
    class Program
    {
        static void Main(string[] args) {
            
            string path =@"E:\com\Ref_4";
            string src  =@"E:\com\ova.xml";
            string dest =@"J:\hdd.raw";
            double target_size = 23676846080;// getSize(src);  //438476734464;
            double size = 0;
            double i=0;
            FileStream fh= new FileStream(dest,FileMode.Create,FileAccess.Write);
            double diff=0;
            int blockSize=1048576;                             //1 meg
            float numOfBocks=(float)target_size/(float)blockSize;
            int maxWrite=blockSize;
            byte[] empty = new byte[blockSize];
            for (int a = 0; a < blockSize; a++) empty[a] = 0;
            
			Message("Source       : Ova.xml",src);            
			Message("Path         : ",path);            
			Message("Size         : ",target_size.ToString());            
			Message("Block Count  : ",numOfBocks.ToString());            

            while(size<target_size){
               string fileName=path+@"\"+String.Format("{0:00000000}",i);
                diff=target_size-size;
                if(diff<blockSize) maxWrite=(int)diff; 
                if(File.Exists(fileName)) {
                    FileStream infile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    byte[] dump= new byte[blockSize];
                    infile.Read(dump, 0,blockSize);
                    infile.Close();
                    infile.Dispose();
                    fh.Write(dump,0,maxWrite);
                }  else {
                    fh.Write(empty,0,maxWrite);
                }
            
                size+=blockSize;
                i++;
                if(i%10==0) {
                    float percent = (float)i / numOfBocks;
					Console.WriteLine(fileName+" "+ percent.ToString()+"% Blocks:"+i+" of "+numOfBocks);
				}
                
            }
            fh.Close();
            
        }

        private static int getSize(string file)
        {
            int size = 1;
            XmlTextReader reader = new XmlTextReader(file);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name == "physical_size") size = 0;
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        if (size == 0) return size = Int32.Parse(reader.Value);
                        break;
                }
            }
            return 0;
        }//end function

        private static void Message(string data1)
        {
            Message("app", data1);
        }

        private static void Message(string data1, string data2)
        {
            Console.ForegroundColor = System.ConsoleColor.Red;
            Console.WriteLine("[" + data1 + "]");
            Console.ResetColor();
            data2 = data2.Replace("\r\n", "\n");
            string[] temp = data2.Split('\n');
            foreach (string x in temp)
            {
                int width = 80 - 9;
                int tlen = x.Length;
                int end = width;
                int start = 0;

                while (start < tlen)
                {
                    if (start + width >= tlen) width = tlen - start;
                    end = start + width;
                    if (start == end) { start++; break; }
                    string str = x.Substring(start, width);
                    Console.WriteLine("        " + str);
                    start += width;
                }
            }
        }//end function
    }//end cass
} //end namespace
