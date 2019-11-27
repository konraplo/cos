using QueryTextDocReportGenerator.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QueryTextDocReportGenerator
{
    public class Program
    {
        private static class SineqaEventType
        {
            public const string SearchText = "search.text";
            public const string SearchResultLink = "click.resultlink";
            public const string DocPreview = "doc.preview";
        }

        public static void Main(string[] args)
        {
            ReadCsvSearchDocs();
        }

        private static void ReadCsvSearchDocs()
        {
            Console.WriteLine("Generate csv report");
            Console.WriteLine("Source environment url: ");
            string sourceEnv = Console.ReadLine();//@"C:\kpl\SearchDocId.csv"

            Console.WriteLine("Source file path: ");
            string source = Console.ReadLine();//@"C:\kpl\SearchDocId.csv"

            Console.WriteLine("Target file path: ");
            string target = Console.ReadLine();//@"C:\kpl\SearchDocIdReport.csv"

            try
            {
                using (StreamReader sr = new StreamReader(source))
                {
                    // currentLine will be null when the StreamReader reaches the end of file

                    List<SinequaProfile> profiles = new List<SinequaProfile>();
                    Dictionary<string, List<SinequaDcoument>> queryDocs = new Dictionary<string, List<SinequaDcoument>>();
                    string currentLine;
                    int i = 0;
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(currentLine))
                        {
                            continue;
                        }

                        if (i == 0)
                        {
                            i++;
                            continue;
                        }

                        string[] coulumns = currentLine.Split(new char[] { ',' });
                        if (coulumns.Length < 5)
                        {
                            continue;
                        }

                        string profile = CleanInput(coulumns[0]);
                        SinequaProfile profileItem = profiles.FirstOrDefault(x => x.Title.Equals(profile, StringComparison.InvariantCultureIgnoreCase));
                        if (profileItem == null)
                        {
                            profileItem = new SinequaProfile();
                            profileItem.Title = profile;
                            profiles.Add(profileItem);
                        }

                        string eventType = CleanInput(coulumns[4]);
                        string resultId = CleanInput(coulumns[2]);
                        if (eventType.Equals(SineqaEventType.SearchText))
                        {
                            string queryText = CleanInput(coulumns[1]);
                            SinequaSearch searchItem = profileItem.SearchItems.FirstOrDefault(x => x.ResultId.Equals(resultId, StringComparison.InvariantCultureIgnoreCase));
                            if (searchItem == null)
                            {

                                searchItem = new SinequaSearch();
                                searchItem.QueryText = queryText;
                                searchItem.ResultId = resultId;
                                searchItem.ItemCount = 1;
                                profileItem.SearchItems.Add(searchItem);
                            }
                        }
                        else if (eventType.Equals(SineqaEventType.SearchResultLink) || eventType.Equals(SineqaEventType.DocPreview))
                        {

                            List<SinequaDcoument> documents;
                            if (!queryDocs.ContainsKey(resultId))
                            {
                                documents = new List<SinequaDcoument>();
                                queryDocs.Add(resultId, documents);
                            }
                            else
                            {
                                documents = queryDocs[resultId];
                            }


                            string documentId = CleanInput(coulumns[3]);
                            SinequaDcoument documentItem = documents.FirstOrDefault(x => x.DocId.Equals(documentId, StringComparison.InvariantCultureIgnoreCase));
                            if (documentItem == null)
                            {

                                documentItem = new SinequaDcoument();
                                documentItem.DocId = documentId;
                                documentItem.ResultId = resultId;
                                sourceEnv = sourceEnv.EndsWith("/") ? sourceEnv.Remove(sourceEnv.Length - 1) : sourceEnv;
                                documentItem.Url = string.Format("{0}/docresult?id={1}", sourceEnv, documentId);
                                documentItem.ItemCount = 1;
                                documents.Add(documentItem);
                            }
                            else
                            {
                                documentItem.ItemCount = ++documentItem.ItemCount;
                            }

                        }

                        i++;
                    }

                    // group by text query and documents
                    foreach (SinequaProfile profileItem in profiles)
                    {
                        if (profileItem.SearchItems.Count() == 0)
                        {
                            continue;
                        }

                        List<SinequaSearch> groupedSarchItems = new List<SinequaSearch>();
                        foreach (SinequaSearch searchItem in profileItem.SearchItems)
                        {
                            if (groupedSarchItems.FirstOrDefault(x => x.QueryText.Equals(searchItem.QueryText, StringComparison.InvariantCultureIgnoreCase)) != null)
                            {
                                continue;
                            }


                            List<SinequaSearch> groupedByText = profileItem.SearchItems.FindAll(x => x.QueryText.Equals(searchItem.QueryText, StringComparison.InvariantCultureIgnoreCase));
                            if (groupedByText.Count() == 1)
                            {
                                groupedSarchItems.Add(searchItem);

                                continue;
                            }

                            SinequaSearch searchItemCumulated = new SinequaSearch();
                            searchItemCumulated.QueryText = searchItem.QueryText;
                            searchItemCumulated.ItemCount = groupedByText.Count();
                            //foreach (SinequaSearch searchItemFound in groupedByText)
                            //{
                            //    if (!queryDocs.ContainsKey(searchItemFound.ResultId))
                            //    {
                            //        continue;
                            //    }
                            //    foreach (SinequaDcoument doc in queryDocs[searchItemFound.ResultId])
                            //    {
                            //        SinequaDcoument docCheck = searchItemCumulated.DocumentItems.FirstOrDefault(x => x.DocId.Equals(doc.DocId, StringComparison.InvariantCultureIgnoreCase));
                            //        if (docCheck == null)
                            //        {
                            //            searchItemCumulated.DocumentItems.Add(doc);
                            //        }
                            //        else
                            //        {
                            //            docCheck.ItemCount = docCheck.ItemCount + doc.ItemCount;
                            //        }
                            //    }
                            //}

                            groupedSarchItems.Add(searchItemCumulated);

                        }

                        foreach (SinequaSearch searchItem in profileItem.SearchItems)
                        {
                            if (!queryDocs.ContainsKey(searchItem.ResultId))
                            {
                                continue;
                            }
                            SinequaSearch searchItemFound = groupedSarchItems.FirstOrDefault(x => x.QueryText.Equals(searchItem.QueryText, StringComparison.InvariantCultureIgnoreCase));
                            if(searchItemFound != null)
                            {
                                
                                foreach (SinequaDcoument doc in queryDocs[searchItem.ResultId])
                                {
                                    SinequaDcoument docCheck = searchItemFound.DocumentItems.FirstOrDefault(x => x.DocId.Equals(doc.DocId, StringComparison.InvariantCultureIgnoreCase));
                                    if (docCheck == null)
                                    {
                                        searchItemFound.DocumentItems.Add(doc);
                                    }
                                    else
                                    {
                                        docCheck.ItemCount = docCheck.ItemCount + doc.ItemCount;
                                    }
                                }
                            }
                        }
                        profileItem.SearchItems.Clear();
                        profileItem.SearchItems.AddRange(groupedSarchItems);

                    }


                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Profil;Suchbegriff;Count;DocId;Count doc;Doc URL");
                    foreach (model.SinequaProfile profileItem in profiles)
                    {
                        foreach (SinequaSearch searchItem in profileItem.SearchItems.OrderByDescending(x => x.ItemCount))
                        {
                            string commodityLine = string.Empty;

                            // query text item
                            commodityLine = string.Format("{0};{1};{2};;", profileItem.Title, searchItem.QueryText, searchItem.ItemCount);
                            sb.AppendLine(commodityLine);

                            // add document items
                            foreach (SinequaDcoument docItem in searchItem.DocumentItems.OrderByDescending(x => x.ItemCount))
                            {
                                commodityLine = string.Empty;

                                // query text item
                                //commodityLine = string.Format(";;;{0};{1};{2}", docItem.DocId, docItem.ItemCount, docItem.Url);
                                commodityLine = string.Format("{0};{1};;{2};{3};{4}", profileItem.Title, searchItem.QueryText, docItem.DocId, docItem.ItemCount, docItem.Url);
                                sb.AppendLine(commodityLine);
                            }
                        }


                    }

                    File.WriteAllText(target, sb.ToString());
                    Console.WriteLine("Report done");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("error:" + e.Message);
            }
            finally
            {
                Console.WriteLine("press any key");
                Console.ReadKey();
            }
        }

        static string CleanInput(string strIn)
        {
            // Replace invalid characters with empty strings.
            try
            {
                return strIn.Replace("\"", string.Empty);
            }
            // If we timeout when replacing invalid characters, 
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return string.Empty;
            }
        }
    }
}
