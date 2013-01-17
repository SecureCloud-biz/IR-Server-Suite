#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using IrssUtils;

namespace MediaPortal.Plugins.IRSS.MPControlPlugin
{
  public class MultiMappings : List<string>
  {
    #region Properties

    [XmlAttribute]
    public int Version
    {
      get { return 4; }
      private set { }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiMappings"/> class.
    /// </summary>
    public MultiMappings()
      : base()
    {
    }

    #endregion Constructors

    #region Static Methods

    /// <summary>
    /// Save the supplied <see cref="MultiMappings"/> to file.
    /// </summary>
    /// <param name="config"><see cref="MultiMappings"/> to save.</param>
    /// <param name="fileName">File to save to.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public static bool Save(MultiMappings config, string fileName)
    {
      try
      {
        XmlSerializer writer = new XmlSerializer(typeof(MultiMappings));
        using (StreamWriter file = new StreamWriter(fileName))
          writer.Serialize(file, config);

        return true;
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        return false;
      }
    }

    /// <summary>
    /// Load <see cref="MultiMappings"/> from a file.
    /// </summary>
    /// <param name="fileName">File to load from.</param>
    /// <returns>Loaded <see cref="MultiMappings"/>.</returns>
    public static MultiMappings Load(string fileName)
    {
      MultiMappings mappings;
      try
      {
        XmlSerializer reader = new XmlSerializer(typeof(MultiMappings));
        using (StreamReader file = new StreamReader(fileName))
          mappings = (MultiMappings)reader.Deserialize(file);
      }
      catch (FileNotFoundException)
      {
        IrssLog.Warn("No multi mapping file found ({0}), creating new MultiMappings", fileName);
        mappings = new MultiMappings();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        IrssLog.Warn("Failed to load multi mapping file ({0}), creating new MultiMappings", fileName);
        mappings = new MultiMappings();
      }

      return mappings;
    }

    #endregion Static Methods
  }
}