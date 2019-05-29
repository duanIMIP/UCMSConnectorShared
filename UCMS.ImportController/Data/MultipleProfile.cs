using IMIP.UniversalScan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

//Using with case Import, export
namespace UCMS.ImportController.Data
{
    public class MultipleProfile
    {
        bool disposed = false;
        public List<String> PathList { get; set; }
        public List<Branch> BranchList { get; set; }
        public List<Model.Folder> FolderList { get; set; }
        public String FileUploadType { get; set; }
        public String FileUploadReName { get; set; }
        public String FileUploadMoveTo { get; set; }
        public String Name { get; set; }
        public Boolean CheckFolder { get; set; }
        public Boolean CheckFile { get; set; }
        public String PathValue { get; set; }
        public Boolean CheckItem { get; set; }

        public MultipleProfile()
        {
            PathList = new List<string>();
            BranchList = new List<Branch>();
            FolderList = new List<Model.Folder>();
            FileUploadType = "";
            FileUploadReName = "";
            FileUploadMoveTo = "";
            Name = "";
            CheckFolder = false;
            CheckFile = false;
            PathValue = "";
            CheckItem = false;
        }

        public MultipleProfile(List<String> PathList, List<Branch> BranchList, List<Model.Folder> FolderList, String FileUploadType, String FileUploadReName, String FileUploadMoveTo, String Name, Boolean CheckFolder, Boolean CheckFile, String PathValue, Boolean CheckItem)
        {
            this.PathList = PathList;
            this.BranchList = BranchList;
            this.FolderList = FolderList;
            this.FileUploadType = FileUploadType;
            this.FileUploadReName = FileUploadReName;
            this.FileUploadMoveTo = FileUploadMoveTo;
            this.Name = Name;
            this.CheckFolder = CheckFolder;
            this.CheckFile = CheckFile;
            this.PathValue = PathValue;
            this.CheckItem = CheckItem;
        }

        ~MultipleProfile()
        {
            PathList = null;
            if (BranchList != null && BranchList.Count> 0) BranchList.Clear();
            BranchList = null;
            if (FolderList != null && FolderList.Count > 0) FolderList.Clear();
            FolderList = null;
            FileUploadType = null;
            FileUploadReName = null;
            FileUploadMoveTo = null;
            Name = null;
            CheckFolder = false;
            CheckFile = false;
            PathValue = null;
            CheckItem = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    PathList = null;
                    if (BranchList != null && BranchList.Count > 0) BranchList.Clear();
                    BranchList = null;
                    if (FolderList != null && FolderList.Count > 0) FolderList.Clear();
                    FolderList = null;
                    FileUploadType = null;
                    FileUploadReName = null;
                    FileUploadMoveTo = null;
                    Name = null;
                    CheckFolder = false;
                    CheckFile = false;
                    PathValue = null;
                    CheckItem = false;
                }
                disposed = true;
            }
        }
    }

    public class MultipleThread
    {
        [XmlElement("PathList")]
        public List<String> PathList { get; set; }
        [XmlElement("BranchList")]
        public List<IMIP.UniversalScan.Data.Branch> BranchList { get; set; }
        [XmlElement("FolderList")]
        public List<DataValue> FolderList { get; set; }
        [XmlElement("FileUploadType")]
        public String FileUploadType { get; set; }
        [XmlElement("FileUploadReName")]
        public String FileUploadReName { get; set; }
        [XmlElement("FileUploadMoveTo")]
        public String FileUploadMoveTo { get; set; }
        [XmlElement("Name")]
        public String Name { get; set; }
        [XmlElement("CheckFolder")]
        public Boolean CheckFolder { get; set; }
        [XmlElement("CheckFile")]
        public Boolean CheckFile { get; set; }
        [XmlElement("PathValue")]
        public String PathValue { get; set; }
        [XmlElement("CheckItem")]
        public Boolean CheckItem { get; set; }
    }
}
