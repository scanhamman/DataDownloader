﻿using CsvHelper;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Html;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace DataDownloader.who
{
    class WHO_Controller
	{
		string sourcefile;
		WHO_Processor processor;
		Source source;		
		string file_base;
		FileWriter file_writer;
		int sf_id;
		int source_id;
		LoggingDataLayer logging_repo;

		public WHO_Controller(string _sourcefile, int _sf_id, Source _source, LoggingDataLayer _logging_repo)
		{
			sourcefile = _sourcefile;
			processor = new WHO_Processor();
			source = _source;
			file_base = source.local_folder;
			source_id = source.id;
			sf_id = _sf_id;
			file_writer = new FileWriter(source);
			logging_repo = _logging_repo;
		}

		public void ProcessFile()
		{
			// WHO processing unusual in that it is from a csv file
			// The program loops through the file and creates an XML file from each row
			// It then distributes it to the correct source folder for 
			// later harvesting.
			// In some cases the file will be one of a set created from a large
			// 'all data' download, in other cases it will be a weekly update file
			// In both cases any existing XML files of the same name 
			// shoud be overwritten

			using (var reader = new StreamReader(sourcefile, true))
			{
				XmlSerializer writer = new XmlSerializer(typeof(WHORecord)); 
				using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
				{
					csv.Configuration.HasHeaderRecord = false;
					var records = csv.GetRecords<WHO_SourceRecord>();
					int n = 0;

					foreach (WHO_SourceRecord sr in records)
					{
						WHORecord r = processor.ProcessStudyDetails(sr, logging_repo);

						n++;
						Console.WriteLine(n);

						if (r != null)
                        {
							// Write out study record as XML, log the download
							string file_base = r.folder_name;
							if (!Directory.Exists(file_base))
                            {
								Directory.CreateDirectory(file_base);
                            }
							string file_name = r.sd_sid + ".xml";
							string full_path = Path.Combine(file_base, file_name);

							file_writer.WriteWHOSourcedFile(writer, r, full_path);
							logging_repo.UpdateDownloadLog(r.source_id, r.sd_sid, r.remote_url, sf_id,
															   DateHelpers.FetchDateTimeFromISO(r.record_date), full_path);

						}
					}
				}
			}
		}
	}
}