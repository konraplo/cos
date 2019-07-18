// --------------------------------------------------------------------------------------------------------------------
// <summary>
// Functionality for zip files creating
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TestConsole
{
    using System;
    using System.IO;
    using System.Text;
    using Ionic.Zip;

    using Microsoft.SharePoint;

    /// <summary>
    /// Class delivers funktionality for zip files creating
    /// </summary>
    public class ZipUtility
    {
        /// <summary>
        /// Generated zip package
        /// </summary>
        private readonly ZipFile zipPackage;

        /// <summary>
        /// Stream object used for serialising of file data
        /// </summary>
        private Stream dataStream = Stream.Null;

        /// <summary>
        /// default or custom package file name
        /// </summary>
        private string pkgName = string.Empty;

        /// <summary>
        /// User has his custom package file name seted
        /// </summary>
        private bool customPkgNameSeted;

        /// <summary>
        /// Package generating DateTime used for default package file name
        /// </summary>
        private DateTime pkgGeneratingDateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipUtility"/> class.
        /// </summary>
        /// <param name="mandantenName">
        /// The mandanten name used in default package file name.
        /// </param>
        /// <param name="documentIdPrefix">
        /// The document id prefix used in default package file name.
        /// </param>
        public ZipUtility(string mandantenName, string documentIdPrefix)
        {
            this.MandantenName = mandantenName;
            this.DocumentenIdPrefix = documentIdPrefix;

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
        /// Gets or sets MandantenName used in default package file name.
        /// </summary>
        public string MandantenName { get; set; }

        /// <summary>
        /// Gets or sets DocumentenIdPrefix used in default package file name.
        /// </summary>
        public string DocumentenIdPrefix { get; set; }

        /// <summary>
        /// Gets or sets PackageName.
        /// Class generates default file name for the zip package.
        /// User can overwrite this default package file name.
        /// </summary>
        /// <remarks>
        /// Property gets generated package file name with last zip saving time stamp.
        /// When FileStream is used as package file output stream, use method GetPackageNameWithTimeStamp(DateTime dateTimeStamp) instead,
        ///    to get the default file name of zip package before file stream opening.
        /// </remarks>
        public string PackageName
        {
            get
            {
                if (!this.customPkgNameSeted)
                {
                    this.pkgName = this.GetDefaultPackageNameWithTimeStamp(this.pkgGeneratingDateTime);
                }

                return this.pkgName;
            }

            set
            {
                this.pkgName = value;
                this.customPkgNameSeted = true;
            }
        }

        /// <summary>
        /// Adds file to generated zip package
        /// </summary>
        /// <param name="fileName">
        /// The added file name.
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
        /// Adds file to generated zip package
        /// </summary>
        /// <param name="fileName">
        /// The added file name.
        /// </param>
        /// <param name="fileBinary">
        /// The file binary data.
        /// </param>
        /// <example>
        /// <code>
        /// ZipUtility zippackage = new ZipUtility("MandantenName", "P00023");
        /// byte[] fileBytes = File.ReadAllBytes(@"c:\\tmp\testFile4.txt");
        /// zippackage.AddFile("testFile4.txt", fileBytes);
        /// </code>
        /// </example>
        public void AddFile(string fileName, byte[] fileBinary)
        {
            ZipEntry entry = this.zipPackage.AddEntry(fileName, fileBinary);            
        }

        /// <summary>
        /// Adds file to generated zip package
        /// Added file will be saved in root folder of the package
        /// </summary>
        /// <param name="sourceFilePath">
        /// Path to the source file.
        /// </param>
        /// <example>
        /// <code>
        /// ZipUtility zippackage = new ZipUtility("MandantenName", "P00023");
        /// zippackage.AddFile(@"c:\\tmp\testFile3.txt");
        /// </code>
        /// </example>
        public void AddFile(string sourceFilePath)
        {
            this.zipPackage.AddFile(sourceFilePath, "\\");
        }

        /// <summary>
        /// Adds file to generated zip package
        /// </summary>
        /// <param name="file">
        /// The share point file object.
        /// </param>
        /// <example>
        /// <code>
        /// ZipUtility zippackage = new ZipUtility("MandantenName", "P00023");
        /// SPFile spFile = web.GetFile("http://dev2010/sites/MySite/MyLibrary/testFile.doc");
        /// zippackage.AddFile(spFile);
        /// </code>
        /// </example>
        public void AddFile(SPFile file)
        {
            this.dataStream = file.OpenBinaryStream();
            this.AddFile(file.Name, this.dataStream);
        }

        public void AddFile(string fileName, Stream fileData, string directoryName = "")
        {
            if (string.IsNullOrEmpty(directoryName))
            {
                this.zipPackage.AddEntry(fileName, fileData);
            }
            else
            {
                ZipEntry entry = this.zipPackage.AddFile(fileName, directoryName);
                Stream str = entry.InputStream;
                byte[] bytes = Encoding.ASCII.GetBytes("dduuppaa");
                str.Write(bytes, 0, (int)fileData.Length);
                str.Flush();
            }
        }

        /// *************************** PR *********************************
        public void AddDirectoryByName(string directoryName)
        {
            this.zipPackage.AddDirectoryByName(directoryName);
        }
        /// *************************** PR *********************************

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
            this.pkgGeneratingDateTime = DateTime.Now;
            this.zipPackage.Save(outStream);
        }

        /// <summary>
        /// Saves generated package to the file system folder
        /// </summary>
        /// <param name="destinationFolder">
        /// The destination folder.
        /// </param>
        /// <example>
        /// <code>
        /// ZipUtility zippackage = new ZipUtility("MandantenName", "P00023");
        /// // add files to package...
        /// // save package to file in given folder
        /// zippackage.SavePackageToFile("c:\\tmp");
        /// </code>
        /// </example>
        public void SavePackageToFile(string destinationFolder)
        {
            this.pkgGeneratingDateTime = DateTime.Now;
            this.zipPackage.Save(destinationFolder + "\\" + this.PackageName);
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
        /// <example>
        /// <code>
        /// ZipUtility zippackage = new ZipUtility("MandantenName", "P00023");
        /// // add fils to package...
        /// // save package to share point library
        /// zzippackage.SavePackageToSharePoint(web, "http://dev2010/sites/MySite/MyLibrary");
        /// </code>
        /// </example>
        public void SavePackageToSharePoint(SPWeb destinationWeb, string destinationFileLocationUrl)
        {
            FileStream streamTmp = null;
            string packageFullName = string.Empty;
            try
            {
                this.pkgGeneratingDateTime = DateTime.Now;
                packageFullName = Path.GetTempPath() + "\\" + this.PackageName;
                streamTmp = File.Create(packageFullName);
                this.SavePackageToStream(streamTmp);
                streamTmp.Flush();
                streamTmp.Close();
                streamTmp = File.OpenRead(packageFullName);
                string destination = destinationFileLocationUrl + (destinationFileLocationUrl.EndsWith("/") ? string.Empty : "/") + this.PackageName;
                destinationWeb.Files.Add(destination, streamTmp);
                streamTmp.Close();
            }
            finally
            {
                if (streamTmp != null)
                {
                    streamTmp.Close();
                }

                if (File.Exists(packageFullName))
                {
                    File.Delete(packageFullName);
                }
            }
        }

        /// <summary>
        /// Generates package file name with given date time stemp
        /// </summary>
        /// <param name="dateTimeStamp">
        /// The date time stamp.
        /// </param>
        /// <returns>
        /// Default package file name with given date time stamp
        /// </returns>
        /// <example>
        /// <c>
        /// string zipFileName = zippackage.GetDefaultPackageNameWithTimeStamp(DateTime.Now);
        /// </c>
        /// </example>
        public string GetDefaultPackageNameWithTimeStamp(DateTime dateTimeStamp)
        {
            return ZipUtilityConst.ZipFileNamePrefix + "_" + this.MandantenName.Substring(0, this.MandantenName.Length < 10 ? this.MandantenName.Length : 10) + "_" + this.DocumentenIdPrefix + "_" + GetFormatedDateTimeString(dateTimeStamp) + ZipUtilityConst.ZipFileNameType;
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
        private static string GetFormatedDateTimeString(DateTime dateTime)
        {
            return string.Format(ZipUtilityConst.GeneratingDateTimeFormat, dateTime, dateTime);
        }
    }
}
