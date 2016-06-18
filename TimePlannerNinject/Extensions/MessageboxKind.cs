// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageboxKind.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// <summary>
//   The kind of message box that should be shown
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TimePlannerNinject.Extensions
{
    /// <summary>
    /// The kind of message box that should be shown
    /// </summary>
    public enum MessageboxKind
    {
        /// <summary>
        /// Only an OK button is shown.
        /// </summary>
        Ok = 0,

        /// <summary>
        /// An OK and cancel button are shown.
        /// </summary>
        OKCancel,

        /// <summary>
        /// Yes no and cancel buttons are shown.
        /// </summary>
        YesNoCancel,

        /// <summary>
        /// Yes and no buttons are shown.
        /// </summary>
        YesNo
    }
}
