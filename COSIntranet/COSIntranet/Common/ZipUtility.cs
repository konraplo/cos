namespace Change.Intranet.Common
{
    using System;

    using System.IO;
    using Ionic.Zip;

    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;

    /// <summary>
    /// Class delivers funktionality for zip files creating
    /// </summary>
    public class ZipUtility
    {
        /// <summary>
        /// File type used in generated zip file name
        /// </summary>
        public static readonly string ZipFileNameType = "zip";

        /// <summary>
        /// default package file title part
        /// </summary>
        public static readonly string PackgageDefaultTitlePart = "ExportedProjectFiles";

        /// <summary>
        /// DateTime format used in generated zip file name
        /// </summary>
        public static readonly string GeneratingDateTimeFormat = "{0:yyyyMMdd}_{1:HHmmss}";

        /// <summary>
        /// Generated zip package
        /// </summary>
        private readonly ZipFile zipPackage;

        /// <summary>
        /// Stream object used for serialising of file data
        /// </summary>
        private Stream dataStream = Stream.Null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipUtility"/> class.
        /// </summary>
        /// <param name="exportedFileTitle">
        /// The compresed file title part.
        /// </param>
        public ZipUtility(string exportedFileTitle)
        {
            this.PackageTitle = exportedFileTitle;

            if (string.IsNullOrEmpty(exportedFileTitle))
            {
                this.PackageTitle = PackgageDefaultTitlePart;
            }
            else
            {
                this.PackageTitle = exportedFileTitle;
            }

            this.zipPackage = new ZipFile();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ZipUtility"/> class. 
        /// </summary>
        ~ZipUtility()
        {
            if (this.zipPackage != null)
            {
                this.zipPackage.Dispose();
            }

            if (this.dataStream != null)
            {
                this.dataStream.Close();
            }
        }

        /// <summary>
        /// Gets or sets title file part used in default package file name.
        /// </summary>
        public string PackageTitle { get; set; }

        public string PackageFullName
        {
            get
            {
                return string.Format("{0}_{1}.{2}",
                    string.IsNullOrEmpty(this.PackageTitle) ? PackgageDefaultTitlePart : this.PackageTitle,
                    this.GetFormatedDateTimeString(DateTime.Now),
                    ZipFileNameType);
            }
        }

        /// <summary>
        /// Generates date time part of the package file name
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// Formated date time string
        /// </returns>
        private string GetFormatedDateTimeString(DateTime dateTime)
        {
            return string.Format(GeneratingDateTimeFormat, dateTime, dateTime);
        }

        /// <summary>
        /// Adds directory to generated zip package by name
        /// </summary>
        /// <param name="directoryName">
        /// The zip package directory name.
        /// </param>
        public void AddDirectoryByName(string directoryName)
        {
            this.zipPackage.AddDirectoryByName(directoryName);
        }

        /// <summary>
        /// Adds directory to generated zip package
        /// </summary>
        /// <param name="directory">
        /// The zip package directory name.
        /// </param>
        public void AddDirectory(string directory)
        {
            this.zipPackage.AddDirectory(directory);
        }

        /// <summary>
        /// Adds file to generated zip package
        /// </summary>
        /// <param name="file">
        /// The share point file object.
        /// </param>
        /// <param name="directoryPathInZip">
        /// Folder path in zip package
        /// </param>
        public void AddFile(SPFile file, string directoryPathInZip)
        {
            string filePathInZip = string.IsNullOrEmpty(directoryPathInZip) ? file.Name : Path.Combine(directoryPathInZip, file.Name);
            this.dataStream = file.OpenBinaryStream();

            this.AddFile(filePathInZip, this.dataStream);
        }

        /// <summary>
        /// Adds file to generated zip package
        /// </summary>
        /// <param name="filePath">
        /// The added file name into zip package path.
        /// </param>
        /// <param name="fileData">
        /// The file data as stream.
        /// </param>
        /// <example>
        /// <code>
        /// ZipUtility zippackage = new ZipUtility("MandantenName", "P00023");
        /// System.IO.FileStream fileToAdd = File.OpenRead(@"c:\tmp\testFile.txt");
        /// zippackage.AddFile("testFile.txt", fileToAdd);
        /// </code>
        /// </example>
        public void AddFile(string fileName, Stream fileData)
        {
            this.zipPackage.AddEntry(fileName, fileData);
        }

        /// <summary>
        /// Saves generated package to the share point location (for example library)
        /// </summary>
        /// <param name="destinationWeb">
        /// The destination web.
        /// </param>
        /// <param name="destinationFileLocationUrl">
        /// The destination file location url.
        /// for example library, folder in the library
        /// </param>
        /// <returns>Name of created zip file</returns>
        /// <example>
        /// <code>
        /// ZipUtility zippackage = new ZipUtility("MandantenName", "P00023");
        /// // add fils to package...
        /// // save package to share point library
        /// zzippackage.SavePackageToSharePoint(web, "http://dev2010/sites/MySite/MyLibrary");
        /// </code>
        /// </example>
        public string SavePackageToSharePoint(SPWeb destinationWeb, string destinationFileLocationUrl)
        {
            FileStream streamTmp = null;
            string tmpPackageFullPath = string.Empty;
            string packageName = string.Empty;
            try
            {
                packageName = this.SavePackageToTempFolder();
                tmpPackageFullPath = Path.Combine(Path.GetTempPath(), packageName);
                streamTmp = File.OpenRead(tmpPackageFullPath);
                string destination = SPUtility.ConcatUrls(destinationFileLocationUrl, packageName);
                destinationWeb.Files.Add(destination, streamTmp);
                streamTmp.Close();
            }
            finally
            {
                if (streamTmp != null)
                {
                    streamTmp.Close();
                }

                if (File.Exists(tmpPackageFullPath))
                {
                    File.Delete(tmpPackageFullPath);
                }
            }

            return packageName;
        }

        /// <summary>
        /// Saves generated package to the temp folder on the server file system
        /// </summary>
        /// <returns>Name of saved zip file</returns>
        public string SavePackageToTempFolder()
        {
            FileStream streamTmp = null;
            string tmpPackageFullPath = string.Empty;
            string packageName = this.PackageFullName;
            try
            {
                tmpPackageFullPath = Path.Combine(Path.GetTempPath(), packageName);
                streamTmp = File.Create(tmpPackageFullPath);
                this.SavePackageToStream(streamTmp);
                streamTmp.Flush();
                streamTmp.Close();
            }
            finally
            {
                if (streamTmp != null)
                {
                    streamTmp.Close();
                }
            }

            return packageName;
        }

        /// <summary>
        /// Saves generated package to external stream
        /// Stream must be opened and closed by the method caller
        /// </summary>
        /// <param name="outStream">
        /// The out stream with generated package data.
        /// </param>
        /// <example>
        /// <code>
        /// //Example 1:
        /// ZipUtility zippackage = new ZipUtility("MandantenName", "P00023");
        /// // add files to package...
        /// // save package to data stream
        /// MemoryStream streamcompr = new MemoryStream();
        /// //get package data
        /// zippackage.SavePackageToStream(streamcompr);
        /// //save geted data to for example file
        /// FileStream fileToSave = new FileStream(@"c:\tmp\" + zippackage.PackageName, FileMode.Create);
        /// streamcompr.WriteTo(fileToSave);
        /// fileToSave.Flush();
        /// fileToSave.Close();
        /// streamcompr.Close();
        /// 
        /// //Example 2:
        /// ZipUtility zippackage = new ZipUtility("MandantenName", "P00023");
        /// // add files to package...
        /// // save package to file stream
        /// string zipFileName = zippackage.GetDefaultPackageNameWithTimeStamp(DateTime.Now);
        /// System.IO.FileStream pckFile = File.Create(@"c:\tmp\" + zipFileName);
        /// zippackage.SavePackageToStream(pckFile);
        /// pckFile.Flush();
        /// pckFile.Close();
        /// </code>
        /// </example>
        public void SavePackageToStream(Stream outStream)
        {
            this.zipPackage.Save(outStream);
        }
    }
}
