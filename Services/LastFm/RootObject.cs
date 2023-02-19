using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LastFm;

public class RootObject
{
    public Toptracks toptracks { get; set; }
}

public class Toptracks
{
    public Track[] track { get; set; }
    public _attr _attr { get; set; }
}

public class Track
{
    public Streamable streamable { get; set; }
    public string mbid { get; set; }
    public string name { get; set; }
    public Image[] image { get; set; }
    public Artist artist { get; set; }
    public string url { get; set; }
    public string duration { get; set; }
    public _attr1 _attr { get; set; }
    public string playcount { get; set; }
}

public class Streamable
{
    public string fulltrack { get; set; }
    public string _text { get; set; }
}

public class Image
{
    public string size { get; set; }
    public string _text { get; set; }
}

public class Artist
{
    public string url { get; set; }
    public string name { get; set; }
    public string mbid { get; set; }
}

public class _attr1
{
    public string rank { get; set; }
}

public class _attr
{
    public string user { get; set; }
    public string totalPages { get; set; }
    public string page { get; set; }
    public string perPage { get; set; }
    public string total { get; set; }
}
