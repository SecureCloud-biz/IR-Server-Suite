using System;
using System.Windows.Forms;

namespace IrssCommands
{
  /// <summary>
  /// Label macro command.
  /// </summary>
  public class CommandLabel : Command
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLabel"/> class.
    /// </summary>
    public CommandLabel()
    {
      InitParameters(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLabel"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public CommandLabel(string[] parameters) : base(parameters)
    {
    }

    #endregion Constructors

    #region Implementation

    /// <summary>
    /// Gets the category of this command.
    /// </summary>
    /// <value>The category of this command.</value>
    public override string Category
    {
      get { return Processor.CategoryControl; }
    }

    /// <summary>
    /// Gets the user interface text.
    /// </summary>
    /// <value>User interface text.</value>
    public override string UserInterfaceText
    {
      get { return "Label"; }
    }

    /// <summary>
    /// Gets the user display text.
    /// </summary>
    /// <value>The user display text.</value>
    public override string UserDisplayText
    {
      get { return String.Format("{0} \"{1}\"", UserInterfaceText, Parameters[0]); }
    }

    /// <summary>
    /// Edit this command.
    /// </summary>
    /// <param name="parent">The parent window.</param>
    /// <returns><c>true</c> if the command was modified; otherwise <c>false</c>.</returns>
    public override bool Edit(IWin32Window parent)
    {
      EditLabel edit = new EditLabel(Parameters[0]);
      if (edit.ShowDialog(parent) == DialogResult.OK)
      {
        Parameters[0] = edit.LabelName;
        return true;
      }

      return false;
    }

    #endregion Implementation
  }
}