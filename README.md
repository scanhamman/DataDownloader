# DataDownloader
Downloads data from mdr data sources to local files, stored as XML.

The functioning of the mdr begins with the creation of a local copy of all the source data. A folder is set up to receive the data, one per source, and the data download process adds files to that folder. The download events are self contained and can take place independently of any further processing. The local copy of the source data simply grows with successive download events. At any point in time the folder holds *all* the data relevant to the mdr from its source, but because the basic details of each file, including the date and time of its download, are recorded later processing stages can select subsets of files from that data store. Sources are trial registries and data repositories, and the download mechanisms used include
* Downloading XML files directly from a source's API, (e.g. for ClicalTrials.gov, PubMed)
* Scraping web pages and generating the XML files from the data obtained (e.g. for ISRCTN, EUCTR, Yoda, BioLincc)
* Downloading CSV files and converting the data into XML files (e.g. for WHO ICTRP data).
The format of the XML files created vary from source to source but represent the initial stage in the process of converting the source data into a consistent schema.<br/><br/>
The program represents the first stage in the 4 stage MDR extraction process:<br/>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**Download** => Harvest => Import => Aggregation<br/><br/>
For a much more detailed explanation of the extraction process,and the MDR system as a whole, please see the project wiki (landing page at https://ecrin-mdr.online/index.php/Project_Overview).<br/>

### Parameters
The system is a console app, and takes the following parameters:<br/>
**-s**, followed by an integer: the id of the source to be downloaded, e.g. 100120 = ClinicalTrials.gov.<br/>
**-t**, followed by an integer: the id of the type of fetch (or sometimes search) - see below for more details.<br/>
**-f**, followed by a file path: the path should be that of the source file for those download types that require it.<br/>
**-d**, followed by a date, in the ISO yyyy-MM-dd format: the date should be the cut-off date for those download data types that require one.<br/>
**-q**, followed by an integer: the integer id of a listed query for using within an API<br/>
**-p**, followed by a string of comma delimited integers: the ids of the previous searches that should be used as the basis of this download.<br/>
**-L**: a flag indicating that no logging should take place. Useful in some testing and development scenarios.<br/>

### Download Types
The range of parameters illustrate the need for the variety of approaches required to deal with the various types of source material. The types of download available, together with the three digit integer id of each, are:<br/>	<br/>	
101:	All records (download)<br/>	
*Identifies and downloads XML files, one per record, from the entire data source. All available files are downloaded.*	

102:	All records (scrape)<br/>	
*Scrapes data, creates XML files, one per record, from the entire data source. All available records are processed.*	

103:	All records (file)<br/>	
*A local file (e.g. CSV file downloaded from WHO) used as the data source. Records transformed into local XML files. Requires file path*

111:	All new or revised records (download)	<br/>
*Identifies and downloads XML files, one per each record that has been added new or revised since the given cutoff date, within the entire data source.	Requires cut off date*

112:	All new or revised records (scrape)<br/>	
*Scrapes data and creates XML files, one per record, from  data that is new or revised since the given cutoff data, inspecting the entire data source. Requires cut off date*

113:	All new or revised records (file)<br/>	
*Uses a downloaded file, with new and / or revised data since the last data fetch (usually as provided by the source). Local XML files created or amended. Requires file path*

114:	All new or revised records with a filter applied (download)<br/>	
*Identifies and downloads XML files, one per each record that has been added new or revised since the given cutoff date, within a filtered source record set.	Requires cut off date, query id*

121:	Filtered records (download)	<br/>
*Identifies and downloads XML files, one per record, that meet specific search criteria (excluding revision date). The search type should be identified.	Requires query id

122:	Filtered records (scrape)	
*Scrapes data and creates XML files, one per record, from data that meets specific search criteria (excluding revision date). The search type should be identified.	Requires query id*

123:	Filtered records (file)	
*Uses a downloaded file, containing data that meets specific search criteria (excluding those based on revision date). Local XML files created or amended. The search type should be identified.	Requires file path, query id*

131:	Records from prior search (download)<br/>	
*Downloads XML files, one per record, that have been previously identified by a search as requiring download. The search(es) should be identified.	Requires previous search id(s)*

132:	Records from prior search (scrape)<br/>	
*Scrapes data, and creates or amends XML files, one per record, that have been previously identified by a search as requiring download. The search(es) should be identified.	Requires previous search id(s)*

133:	Records from prior search (file)<br/>	
*Uses a downloaded file and extracts from it data that has previously been identified as required by a search. The search(es) should be identified.	requires file path, previous search id(s)*

134:	Records from prior search and new or revised records (download)	<br/>
*Downloads XML files, one per record, that have been previously identified by a search as requiring download, and / or that have been revised or added since the cutoff date. The search(es) should be identified.	requires cut off date, previous search id(s)*

141:	Assumed incomplete records (download)	<br/>
*Identifies and downloads XML files, one per each record, that are assumed to be incomplete (using source specific criteria), within the entire data source.*

142:	Assumed incomplete records (scrape)	<br/>
*Scrapes data and creates XML files, one per record, from  data that is assumed to be incomplete (using source-specific criteria), inspecting the entire data source.*

143:	Assumed incomplete records (file)	<br/>
*Uses a downloaded file, with data that is assumed to be incomplete (using source specific criteria). Local XML files created or amended.	Requires file path*

201:	Full search (records located only)<br/>	
*Identifies data or web page, including its source URL, across the entire data source, for later fetch.*	

202:	New or revised records (records located only)<br/>	
*Identifies data or web page, including its source URL, that meet the criteria of having been revised or added since the given cutoff date. For later fetch. Requires cut off date*

203:	Filtered search (records located only)<br/>	
*Identifies data or web page, including its source URL, that meet the criteria of a focused search, for later fetch. The search type should be specified.	Requires query id*

204:	New or revised records and filtered search  (records located only)	
*Identifies data or web page, including its source URL, that meet the criteria of a focused search, AND which have also been revised or added since the given cutoff date, for later fetch.	Requires cut off date, query id*

205:	Search using MDR data (records located only)<br/>	
*Identifies data or web pages, including their source URLs, where previously processed MDR data indicates it should be fetched, e.g. references in one source to another.	Requires query id*

### Query types
The types of wquery are likely to grow with time as different sources are used. At the moment the main use for these filters is with PubMed data. The current filter quesrties used are:

### Overview

### Logging
Logging of data dowwnload is critical because itr provides the basis for orchestrating processes later on in the extractioon pathway. A record is created for each study that is downloaded (in study based sources like trial registries) or for each data object downloaded (for object based resources like PubMed) a **'data source record'** is established. This includes:
* the source id, 
* the object's own id, in the source data (e.g. a registry identifier), 
* the URL of its record on the web - if it has one 
* the local path where the XML file downloaded or created is stored
* the datetime that the record was last revised, if available
* a boolean indicating if the record is assumed complete (used when no revision date is available)
* the download status - an integer - where 0 indicates found in a search but not yet (re)downloaded, and 2 indicates downloaded.
* the id of the fetch / search event in which it was last downloaded / created
* the date time of that fetch / search
* the id of the harvest event in which it was last harvested
* the date time of that harvest
* the id of the import event in which it was last imported
* the date time of that import

In other words the source record provides, for each individual downloaded entity, a record of their current status in the system.<br/>	
During a fetch / save event new studies (or objects for PubMed) will generate new records in this table. Existing records will update the records - possibly updating the date last revised as well as the data was last fetched.<br/>	 

### Provenance
* Author: Steve Canham
* Organisation: ECRIN (https://ecrin.org)
* System: Clinical Research Metadata Repository (MDR)
* Project: EOSC Life
* Funding: EU H2020 programme, grant 824087

