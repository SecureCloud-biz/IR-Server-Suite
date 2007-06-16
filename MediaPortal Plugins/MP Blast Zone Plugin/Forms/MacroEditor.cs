using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using MediaPortal.GUI.Library;
using MediaPortal.Util;

using IrssUtils;
using IrssUtils.Forms;

namespace MediaPortal.Plugins
{

  public partial class MacroEditor : Form
  {

    #region Constructor

    /// <summary>
    /// Creates a Macro Editor windows form.
    /// </summary>
    /// <param name="newMacro">This a new macro.</param>
    /// <param name="name">The name of this macro.</param>
    public MacroEditor(bool newMacro, string name)
    {
      InitializeComponent();

      textBoxName.Text    = name;
      textBoxName.Enabled = newMacro;

      if (!newMacro)
      {
        string fileName = MPBlastZonePlugin.FolderMacros + name + Common.FileExtensionMacro;
        ReadFromFile(fileName);
      }
    }

    #endregion Constructor

    #region Implementation

    void RefreshCommandList()
    {
      comboBoxCommands.Items.Clear();

      comboBoxCommands.Items.Add(Common.UITextRun);
      comboBoxCommands.Items.Add(Common.UITextPause);
      comboBoxCommands.Items.Add(Common.UITextSerial);
      comboBoxCommands.Items.Add(Common.UITextWindowMsg);
      comboBoxCommands.Items.Add(Common.UITextGoto);
      comboBoxCommands.Items.Add(Common.UITextPopup);
//    comboBoxCommands.Items.Add(Common.UITextWindowState);
      comboBoxCommands.Items.Add(Common.UITextFocus);
      comboBoxCommands.Items.Add(Common.UITextExit);
      comboBoxCommands.Items.Add(Common.UITextStandby);
      comboBoxCommands.Items.Add(Common.UITextHibernate);
      comboBoxCommands.Items.Add(Common.UITextReboot);
      comboBoxCommands.Items.Add(Common.UITextShutdown);

      comboBoxCommands.Items.AddRange(Common.GetIRList(true));
    }

    /// <summary>
    /// Write the macro in the listBox to a macro name provided.
    /// </summary>
    /// <param name="fileName">Name of Macro to write (macro name, not file path).</param>
    void WriteToFile(string fileName)
    {
      XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
      writer.Formatting = Formatting.Indented;
      writer.Indentation = 1;
      writer.IndentChar = (char)9;
      writer.WriteStartDocument(true);
      writer.WriteStartElement("macro"); // <macro>

      foreach (string item in listBoxMacro.Items)
      {
        writer.WriteStartElement("action");

        if (item.StartsWith(Common.CmdPrefixBlast))
        {
          writer.WriteAttributeString("command", Common.XmlTagBlast);
          writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixBlast.Length));
        }
        else if (item.StartsWith(Common.CmdPrefixPause))
        {
          writer.WriteAttributeString("command", Common.XmlTagPause);
          writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixPause.Length));
        }
        else if (item.StartsWith(Common.CmdPrefixRun))
        {
          writer.WriteAttributeString("command", Common.XmlTagRun);
          writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixRun.Length));
        }
        else if (item.StartsWith(Common.CmdPrefixGoto))
        {
          writer.WriteAttributeString("command", Common.XmlTagGoto);
          writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixGoto.Length));
        }
        else if (item.StartsWith(Common.CmdPrefixSerial))
        {
          writer.WriteAttributeString("command", Common.XmlTagSerial);
          writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixSerial.Length));
        }
        else if (item.StartsWith(Common.CmdPrefixWindowMsg))
        {
          writer.WriteAttributeString("command", Common.XmlTagWindowMsg);
          writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixWindowMsg.Length));
        }
        else if (item.StartsWith(Common.CmdPrefixPopup))
        {
          writer.WriteAttributeString("command", Common.XmlTagPopup);
          writer.WriteAttributeString("cmdproperty", item.Substring(Common.CmdPrefixPopup.Length));
        }
/*
        else if (item.StartsWith(Common.CmdPrefixWindowState))
        {
          writer.WriteAttributeString("command", Common.XmlTagWindowState);
          writer.WriteAttributeString("cmdproperty", String.Empty);
        }
*/
        else if (item.StartsWith(Common.CmdPrefixFocus))
        {
          writer.WriteAttributeString("command", Common.XmlTagFocus);
          writer.WriteAttributeString("cmdproperty", String.Empty);
        } 
        else if (item.StartsWith(Common.CmdPrefixExit))
        {
          writer.WriteAttributeString("command", Common.XmlTagExit);
          writer.WriteAttributeString("cmdproperty", String.Empty);
        }
        else if (item.StartsWith(Common.CmdPrefixStandby))
        {
          writer.WriteAttributeString("command", Common.XmlTagStandby);
          writer.WriteAttributeString("cmdproperty", String.Empty);
        }
        else if (item.StartsWith(Common.CmdPrefixHibernate))
        {
          writer.WriteAttributeString("command", Common.XmlTagHibernate);
          writer.WriteAttributeString("cmdproperty", String.Empty);
        }
        else if (item.StartsWith(Common.CmdPrefixReboot))
        {
          writer.WriteAttributeString("command", Common.XmlTagReboot);
          writer.WriteAttributeString("cmdproperty", String.Empty);
        }
        else if (item.StartsWith(Common.CmdPrefixShutdown))
        {
          writer.WriteAttributeString("command", Common.XmlTagShutdown);
          writer.WriteAttributeString("cmdproperty", String.Empty);
        }

        writer.WriteEndElement();
      }

      writer.WriteEndElement(); // </macro>
      writer.WriteEndDocument();
      writer.Close();
    }

    /// <summary>
    /// Read a stored Macro from the macro name provided.
    /// </summary>
    /// <param name="fileName">Name of Macro to read from (macro name, not file path).</param>
    void ReadFromFile(string fileName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      listBoxMacro.Items.Clear();

      XmlNodeList commandSequence = doc.DocumentElement.SelectNodes("action");

      string commandProperty;
      foreach (XmlNode item in commandSequence)
      {
        commandProperty = item.Attributes["cmdproperty"].Value;

        switch (item.Attributes["command"].Value)
        {
          case Common.XmlTagBlast:
            listBoxMacro.Items.Add(Common.CmdPrefixBlast + commandProperty);
            break;

          case Common.XmlTagPause:
            listBoxMacro.Items.Add(Common.CmdPrefixPause + commandProperty);
            break;

          case Common.XmlTagRun:
            listBoxMacro.Items.Add(Common.CmdPrefixRun + commandProperty);
            break;

          case Common.XmlTagSerial:
            listBoxMacro.Items.Add(Common.CmdPrefixSerial + commandProperty);
            break;

          case Common.XmlTagWindowMsg:
            listBoxMacro.Items.Add(Common.CmdPrefixWindowMsg + commandProperty);
            break;

          case Common.XmlTagGoto:
            listBoxMacro.Items.Add(Common.CmdPrefixGoto + commandProperty);
            break;

          case Common.XmlTagPopup:
            listBoxMacro.Items.Add(Common.CmdPrefixPopup + commandProperty);
            break;
/*
          case Common.XmlTagWindowState:
            listBoxMacro.Items.Add(Common.CmdPrefixWindowState);
            break;
*/
          case Common.XmlTagFocus:
            listBoxMacro.Items.Add(Common.CmdPrefixFocus);
            break;

          case Common.XmlTagExit:
            listBoxMacro.Items.Add(Common.CmdPrefixExit);
            break;

          case Common.XmlTagStandby:
            listBoxMacro.Items.Add(Common.CmdPrefixStandby);
            break;

          case Common.XmlTagHibernate:
            listBoxMacro.Items.Add(Common.CmdPrefixHibernate);
            break;

          case Common.XmlTagReboot:
            listBoxMacro.Items.Add(Common.CmdPrefixReboot);
            break;

          case Common.XmlTagShutdown:
            listBoxMacro.Items.Add(Common.CmdPrefixShutdown);
            break;
        }
      }
    }

    private void MacroEditor_Load(object sender, EventArgs e)
    {
      RefreshCommandList();
    }

    private void buttonAddCommand_Click(object sender, EventArgs e)
    {
      if (comboBoxCommands.SelectedIndex == -1)
        return;

      string selected = comboBoxCommands.SelectedItem as string;

      if (selected == Common.UITextRun)
      {
        ExternalProgram externalProgram = new ExternalProgram();
        if (externalProgram.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixRun + externalProgram.CommandString);
      }
      else if (selected == Common.UITextPause)
      {
        IrssUtils.Forms.PauseTime pauseTime = new IrssUtils.Forms.PauseTime();
        if (pauseTime.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixPause + pauseTime.Time.ToString());
      }
      else if (selected == Common.UITextSerial)
      {
        SerialCommand serialCommand = new SerialCommand();
        if (serialCommand.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixSerial + serialCommand.CommandString);
      }
      else if (selected == Common.UITextWindowMsg)
      {
        MessageCommand messageCommand = new MessageCommand();
        if (messageCommand.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixWindowMsg + messageCommand.CommandString);
      }
      else if (selected == Common.UITextGoto)
      {
        MPUtils.Forms.GoToScreen goToScreen = new MPUtils.Forms.GoToScreen();
        if (goToScreen.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixGoto + goToScreen.Screen);
      }
      else if (selected == Common.UITextPopup)
      {
        PopupMessage popupMessage = new PopupMessage();
        if (popupMessage.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixPopup + popupMessage.CommandString);
      }
/*
      else if (selected == Common.UITextWindowState)
      {
        listBoxMacro.Items.Add(Common.CmdPrefixWindowState);
      }
*/
      else if (selected == Common.UITextFocus)
      {
        listBoxMacro.Items.Add(Common.CmdPrefixFocus);
      }
      else if (selected == Common.UITextExit)
      {
        listBoxMacro.Items.Add(Common.CmdPrefixExit);
      }
      else if (selected == Common.UITextStandby)
      {
        listBoxMacro.Items.Add(Common.CmdPrefixStandby);
      }
      else if (selected == Common.UITextHibernate)
      {
        listBoxMacro.Items.Add(Common.CmdPrefixHibernate);
      }
      else if (selected == Common.UITextReboot)
      {
        listBoxMacro.Items.Add(Common.CmdPrefixReboot);
      }
      else if (selected == Common.UITextShutdown)
      {
        listBoxMacro.Items.Add(Common.CmdPrefixShutdown);
      }
      else if (selected.StartsWith(Common.CmdPrefixBlast))
      {
        BlastCommand blastCommand = new BlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));
        if (blastCommand.ShowDialog(this) == DialogResult.OK)
          listBoxMacro.Items.Add(Common.CmdPrefixBlast + blastCommand.CommandString);
      }
      else
      {
        throw new Exception(String.Format("Unknown command in macro command list \"{0}\"", selected));
      }
    }

    private void buttonMoveUp_Click(object sender, EventArgs e)
    {
      int selected = listBoxMacro.SelectedIndex;
      if (selected > 0)
      {
        object item = listBoxMacro.Items[selected];
        listBoxMacro.Items.RemoveAt(selected);
        listBoxMacro.Items.Insert(selected - 1, item);
        listBoxMacro.SelectedIndex = selected - 1;
      }
    }
    private void buttonMoveDown_Click(object sender, EventArgs e)
    {
      int selected = listBoxMacro.SelectedIndex;
      if (selected < listBoxMacro.Items.Count - 1)
      {
        object item = listBoxMacro.Items[selected];
        listBoxMacro.Items.RemoveAt(selected);
        listBoxMacro.Items.Insert(selected + 1, item);
        listBoxMacro.SelectedIndex = selected + 1;
      }
    }

    private void buttonRemove_Click(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex != -1)
        listBoxMacro.Items.RemoveAt(listBoxMacro.SelectedIndex);
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      string fileName = textBoxName.Text.Trim();

      if (fileName.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this macro", "Name missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      fileName = MPBlastZonePlugin.FolderMacros + fileName + Common.FileExtensionMacro;

      WriteToFile(fileName);

      try
      {
        MPBlastZonePlugin.ProcessMacro(MPBlastZonePlugin.FolderMacros + fileName + Common.FileExtensionMacro);
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      string fileName = textBoxName.Text.Trim();

      if (fileName.Length == 0)
      {
        MessageBox.Show(this, "You must supply a name for this macro", "Name missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return;
      }

      fileName = MPBlastZonePlugin.FolderMacros + fileName + Common.FileExtensionMacro;

      WriteToFile(fileName);

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void listBoxCommandSequence_DoubleClick(object sender, EventArgs e)
    {
      if (listBoxMacro.SelectedIndex != -1)
      {
        string selected = listBoxMacro.SelectedItem as string;

        if (selected.StartsWith(Common.CmdPrefixPause))
        {
          IrssUtils.Forms.PauseTime pauseTime = new IrssUtils.Forms.PauseTime(int.Parse(selected.Substring(Common.CmdPrefixPause.Length)));

          if (pauseTime.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixPause + pauseTime.Time.ToString());
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixRun))
        {
          string[] commands = Common.SplitRunCommand(selected.Substring(Common.CmdPrefixRun.Length));
         
          ExternalProgram executeProgram = new ExternalProgram(commands);
          if (executeProgram.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixRun + executeProgram.CommandString);
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixSerial))
        {
          string[] commands = Common.SplitSerialCommand(selected.Substring(Common.CmdPrefixSerial.Length));

          SerialCommand serialCommand = new SerialCommand(commands);
          if (serialCommand.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixSerial + serialCommand.CommandString);
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixWindowMsg))
        {
          string[] commands = Common.SplitWindowMessageCommand(selected.Substring(Common.CmdPrefixWindowMsg.Length));

          MessageCommand messageCommand = new MessageCommand(commands);
          if (messageCommand.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixWindowMsg + messageCommand.CommandString);
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixGoto))
        {
          MPUtils.Forms.GoToScreen goToScreen = new MPUtils.Forms.GoToScreen(selected.Substring(Common.CmdPrefixGoto.Length));
          if (goToScreen.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixGoto + goToScreen.Screen);
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixPopup))
        {
          string[] commands = Common.SplitPopupCommand(selected.Substring(Common.CmdPrefixPopup.Length));

          PopupMessage popupMessage = new PopupMessage(commands);
          if (popupMessage.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixPopup + popupMessage.CommandString);
          listBoxMacro.SelectedIndex = index;
        }
        else if (selected.StartsWith(Common.CmdPrefixBlast))
        {
          string[] commands = Common.SplitBlastCommand(selected.Substring(Common.CmdPrefixBlast.Length));

          BlastCommand blastCommand = new BlastCommand(commands);
          if (blastCommand.ShowDialog(this) == DialogResult.Cancel)
            return;

          int index = listBoxMacro.SelectedIndex;
          listBoxMacro.Items.RemoveAt(index);
          listBoxMacro.Items.Insert(index, Common.CmdPrefixBlast + blastCommand.CommandString);
          listBoxMacro.SelectedIndex = index;
        }
      }
    }

    #endregion Implementation

  }

}
