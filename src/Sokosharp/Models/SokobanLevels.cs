using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SokoSharp.Models
{
	[XmlRoot(ElementName = "Level")]
	public class Level
	{
		[XmlElement(ElementName = "L")]
		public List<string> L { get; set; }
		[XmlAttribute(AttributeName = "Id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "Width")]
		public int Width { get; set; }
		[XmlAttribute(AttributeName = "Height")]
		public int Height { get; set; }
	}

	[XmlRoot(ElementName = "LevelCollection")]
	public class LevelCollection
	{
		[XmlElement(ElementName = "Level")]
		public List<Level> Level { get; set; }
		[XmlAttribute(AttributeName = "Copyright")]
		public string Copyright { get; set; }
		[XmlAttribute(AttributeName = "MaxWidth")]
		public string MaxWidth { get; set; }
		[XmlAttribute(AttributeName = "MaxHeight")]
		public string MaxHeight { get; set; }
	}

	[XmlRoot(ElementName = "SokobanLevels")]
	public class SokobanLevels
	{
		[XmlElement(ElementName = "Title")]
		public string Title { get; set; }
		[XmlElement(ElementName = "Description")]
		public string Description { get; set; }
		[XmlElement(ElementName = "Email")]
		public string Email { get; set; }
		[XmlElement(ElementName = "LevelCollection")]
		public LevelCollection LevelCollection { get; set; }
		[XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
		public string Xsi { get; set; }
		[XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
		public string SchemaLocation { get; set; }
	}
}
		