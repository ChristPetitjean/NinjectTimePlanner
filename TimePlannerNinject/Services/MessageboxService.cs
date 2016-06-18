using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Services
{
    using System.Windows;

    using TimePlannerNinject.Extensions;
    using TimePlannerNinject.Model;

    /// <summary>
    /// Concrete implementation of the <see cref="IMessageboxService"/> for WPF.
    /// </summary>
    public class MessageboxService : IMessageboxService
    {
        /// <inheritdoc />
        public MessageboxResponse ShowMessagebox(string message, MessageboxKind messageboxKind, string title = null)
        {
            var result = MessageBox.Show(message, title, GetButtonFromMessageBoxKind(messageboxKind));
            return GetMessageboxResponceFromResult(result);
        }

        /// <summary>
        /// Converts the <see cref="MessageBoxResult"/> enum to its equivalent <see cref="MessageboxResponse"/> value.
        /// </summary>
        /// <param name="messageboxResult">
        /// The native message box result
        /// </param>
        /// <returns>
        /// The message box response
        /// </returns>
        private static MessageboxResponse GetMessageboxResponceFromResult(MessageBoxResult messageboxResult)
        {
            switch (messageboxResult)
            {
                case MessageBoxResult.Cancel:
                    return MessageboxResponse.Cancel;

                case MessageBoxResult.No:
                    return MessageboxResponse.No;

                case MessageBoxResult.None:
                    return MessageboxResponse.None;

                case MessageBoxResult.OK:
                    return MessageboxResponse.Ok;

                case MessageBoxResult.Yes:
                    return MessageboxResponse.Yes;

                default:
                    throw new ArgumentException(string.Format("Unsupported message box result '{0}'", messageboxResult), messageboxResult.ToString());
            }
        }

        /// <summary>
        /// Converts the <see cref="MessageboxKind"/> enum into its equivalent <see cref="MessageBoxButton"/> value.
        /// </summary>
        /// <param name="messageboxKind">The message box kind</param>
        /// <returns>
        /// The equivalent <see cref="MessageBoxButton"/>
        /// </returns>
        private static MessageBoxButton GetButtonFromMessageBoxKind(MessageboxKind messageboxKind)
        {
            switch (messageboxKind)
            {
                case MessageboxKind.Ok:
                    return MessageBoxButton.OK;

                case MessageboxKind.OKCancel:
                    return MessageBoxButton.OKCancel;

                case MessageboxKind.YesNo:
                    return MessageBoxButton.YesNo;

                case MessageboxKind.YesNoCancel:
                    return MessageBoxButton.YesNoCancel;

                default:
                    throw new ArgumentException(string.Format("Unsupported message box kind '{0}'", messageboxKind), messageboxKind.ToString());
            }
        }
    }
}
