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

            new FileAPI.
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

        public string coombineToEnd(int end) {
            int index = 0;
            string fin = "";
            foreach (string s in seperated) {
                if(index <= end){
                    fin = fin+"/"+s;
                }
                index++;
            }
            return fin;
        }

        //TODO: From Start
    }

    class FileSecurityManager {

        private File file;
        private FileAttributes attributes;
        public FileSecurityManager(File file) {
            this.file = file;
           // this.attributes = System.IO.File.GetAttributes(file.getPath());
        }
    }

    abstract class FileFilter {

        public abstract bool pass(File file);

    }

    class FileExtensionFilter : FileFilter {
        
        private string extension;
        
        public FileExtensionFilter(string extension) {
            this.extension = extension;
        }

        public override bool pass(File file) {
            if(file.getFileExtension().Equals(extension)) return true;
            else return false;
        }
    }
    class File {

        private string path;
        private PathSeparator pathSeparator;
        private string name;
        private List<File> files;
        private FileSecurityManager securityManager;

        public File(string path) {
            this.securityManager = new FileSecurityManager(this);
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

        public bool createFile() {
            
            try
            {
                System.IO.File.Create(path);
                return true;    
            }
            catch (System.Exception e)
            {
                
                throw new System.IO.IOException(e.Message);
            }
        }

        public bool mkdir() {
            try {
                System.IO.Directory.CreateDirectory(path);
                return true;
            }
            catch(System.Exception) {
                return false;
            }
        }

        public bool mkdirs() {
            try {
                int index = 0;
                foreach(string p in pathSeparator.getSeperated()) {
                    string com = pathSeparator.coombineToEnd(index);
                    if(!System.IO.Directory.Exists(com)) {
                        System.IO.Directory.CreateDirectory(com);
                    }
                    index++;
                }
                return true;
            }catch(System.Exception e) {
                return false;
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

