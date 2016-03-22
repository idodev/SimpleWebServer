# Simple Web Server

A (very) minimal command line html webserver written in vb.NET

## Usage

*TIP: Copy SimpleWebServer.exe into `C:\Windows\System32` to be able to call the executable from any location*

The executable takes two parameters, the path to the web root folder and a local port

```
	SimpleWebServer "C:\test-website" 8000
```

The path specified can be either absolute or relative. If you were in the web root the following would also work:

```
    SimpleWebServer . 8001
```