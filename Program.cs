using System;
using System.IO;
using System.Collections.Generic;
namespace FileManagerC_ {
    class Program
    {
        static void Main(string[] args) {
            foreach (FileAPI.File file in new FileAPI.File("/mnt/0E9E8E9B9E8E7B4B/lol").listFiles())
            {
                Console.WriteLine(file.getName());               
            }
        }
    }
}


namespace FileAPI {


    class PathSeparator {

        private File file;
        private string[] seperated;
        public PathSeparator(File file) {
            this.file = file;
            this.seperated = file.getPath().Split("/");
        }

        public string[] getSeperated() {
            return seperated;
        }

        public File getFile() {
            return file;
        }
    }
    class File {

        private string path;
        private PathSeparator pathSeparator;
        private string name;
        private List<File> files;
        public File(string path) {
            this.path = path.Replace(@"\","/",true,null);
            this.pathSeparator = new PathSeparator(this);
            this.name = pathSeparator.getSeperated()[pathSeparator.getSeperated().Length-1];

            if(isDirectory()) {
                List<File> filesA = new List<File>();
                foreach(string p in System.IO.Directory.GetFiles(path)){
                    filesA.Add(new File(p));
                }
                foreach (string i in System.IO.Directory.GetDirectories(this.path))
                {
                    filesA.Add(new File(i));
                }
                
                this.files = filesA;
            }else{
                this.files = null;
            }
        }

        public string getPath() {
            return path;
        }

        public string getName() {
            return name;
        }

        public bool exists() {
            try
            {
                FileAttributes attr = System.IO.File.GetAttributes(path);
                return true;
            }
            catch (System.Exception)
            {
                
                return false;
            }
        }

        public bool isDirectory() {
            try
            {
                FileAttributes attr = System.IO.File.GetAttributes(path);

                return attr.HasFlag(FileAttributes.Directory);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool isFile() {
            try
            {
                FileAttributes attr = System.IO.File.GetAttributes(path);

                return !attr.HasFlag(FileAttributes.Directory);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public string getFileExtension() {
            if(!isDirectory()){
                try
                {
                    FileAttributes attr = System.IO.File.GetAttributes(path);

                    return name.Substring(name.LastIndexOf(".")+1);
                }
                catch (System.Exception)
                {
                    return null;
                }
            }else{
                return null;
            }
        }

        public List<File> listFiles() {
            
            return files;
        }

    }

}

