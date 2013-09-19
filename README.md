LogPipe
=======
LogPipe is a windows service collecting log data from multiple sources and relaying it to a centralized storage, Elasticsearch. Much similar to the popular Logstash, and just like with Logstash Kibana can be used to read all data collected in Elasticsearch.

#Getting Started
###Installation
LogPipe uses TopShelf (http://topshelf-project.com/) to simplify its installation process.

* To install run: `Consortio.LogPipe.Host.exe install`
* To uninstall run: `Consortio.LogPipe.Host.exe uninstall`

For more information http://docs.topshelf-project.com/en/latest/overview/commandline.html

###Concepts
Important concepts to understand in LogPipe; inputs, pipeline, events, filters and outputs.

####Inputs
Inputs are used to specify a particular type of log, for example a set of rolling files for MyApp.
Currently LogPipe only support file inputs. For each input you must provide a type name and a path.
The type name will later be used to determine what filters to apply and to which outputs to send the logs.
The input path can point to a specific log file or to a directory of log files. Wild cards can be used to only target files matching a given pattern, for example "*.log".

In addition to type name you can also associate tags with an input. Tags can be used just like type name when applying filters and outputs.

LogPipe will watch all defined inputs and create event objects for all log entries it detects.
For a file input a new event object is created for every linebreak.
LogPipe also keeps track of the last processed line number for each file to provide catch up functionallity in case you want to read old log files or if you must stop the LogPipe service for maintenance.
It's also used to pause the input in case the processing of filters and outputs later in the pipeline is slower than the input.

Once an event object is created it's passed down the pipeline to be processed by any matching filters and outputs. 

####Pipeline
A pipeline is created for every input type. It contains all filters and outputs matching that particular input. It will control the flow of how an event object is processed.

####Events
An event object contains the context of a log entry through out the pipeline as the original log entry is being processed.
The event object will in addition to the original log entry contain information about:
* Source - The file path from which the event originated.
* Type - The type name for the input.
* Timestamp - The time for when the log entry was processed, OR if the ExtractTimestampFilter is used the value which is defined within the log entry it self.
* Message - The original log message.
* Tags - A list of tags added by either the input or a filter.
* Fields - A list of fields (key-value pair) extracted by a filter.

####Filters
Filters are used to manipulate the raw message of a log entry. It can replace/transform the original message or it can extract parts of the message to fields or tags. Fields and tags are much easiser to search, filter and drilldown on when browsing your logs in Elasticsearch/Kibana.

LogPipe currently has 4 built-in filters:

* ExtractTimestampFilter - Trys to extract a datetime from the log message. 2013-09-19 12:04:56Z for UTC, 2013-09-19 12:04:56 for local time. The extracted value will be parsed using DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ssK", DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal) and saved event.Timestamp and a field called "event_time".
* MatchFilter
* DropFilter
* MultiLineFilter

####Outputs
LogPipe only has a single output which stores the events in ElasticSearch. The events will be stored using the same format as Logstash making all data readable by Kibana. 

####Patterns

###Configuration
Configuration can be found in App.config of the Log Pipe Service project.

####Inputs
```
<LogPipe>
  <Input>
    <File Type="iislogs">
      <Path>C:\inetpub\logs\LogFiles\W3SVC1\*.log</Path>
      <Tags>
        <Tag>tag1</Tag>
      </Tags>
    </File>
    <File Type="myapp">
      <Path>C:\myapp\log4net\*.log</Path>
      <Tags>
        <Tag>tag2</Tag>
      </Tags>
    </File>
  </Input>
</LogPipe>
```
      
####Filters
```
<LogPipe>
  <Filter>
    <ExtractTimestampFilter />

    <MultiLineFilter>
      <Conditions>
        <Type>myapp</Type>
        <Expression>^(DEBUG|WARN|ERROR|INFO|FATAL)</Expression>
      </Conditions>
    </MultiLineFilter>

    <DropFilter>
      <Conditions>
        <Type>iislogs</Type>
        <Expression><![CDATA[(^#)|(?i:/assets/)|(?i:/resources/)]]></Expression>
      </Conditions>
    </DropFilter>

    <MatchFilter>
      <Conditions>
        <Type>iislogs</Type>
        <Expression><![CDATA[^%{DATESTAMP:event_time} %{WORD:sitename} %{HOST:server_name} %{IP:server_ip} %{WORD:http_method} %{URIPATH:uri_path} (?:%{DATA:uri_params}|-) %{NUMBER:port} (?:%{USER:username}|-) %{IPORHOST:client_ip} (?:HTTP/%{NUMBER:http_version}) (?:%{DATA:user_agent}|-) (?:%{DATA:cookie}|-) (?:%{URI:referer}|-) %{IPORHOST:host} %{NUMBER:status} %{NUMBER:substatus} %{NUMBER:win32status} %{NUMBER:bytes_sent} %{NUMBER:bytes_received} %{NUMBER:time_taken}]]></Expression>
      </Conditions>
      <Actions>
        <ExtractFields />
      </Actions>
    </MatchFilter>
    
    <MatchFilter>
      <Conditions>
        <Type>iislogs</Type>
        <Expression><![CDATA[ASP.NET_SessionId=%{WORD:session_id};]]></Expression>
      </Conditions>
      <Actions>
        <ExtractFields />
      </Actions>
    </MatchFilter>
  </Filter>
</LogPipe>
```

####Outputs
```
<LogPipe>
  <Output>
    <ElasticSearch>
      <Host>127.0.0.1</Host>
      <Port>9200</Port>
      <IndexNameFormat>\l\o\g\p\i\p\e\-yyyyMM</IndexNameFormat>
    </ElasticSearch>
  </Output>
</LogPipe>
```
