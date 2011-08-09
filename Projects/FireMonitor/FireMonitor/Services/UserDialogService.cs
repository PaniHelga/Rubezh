﻿using System.Windows;
using Infrastructure.Common;

namespace FireMonitor
{
    public class UserDialogService : IUserDialogService
    {
        public bool ShowWindow(IDialogContent model)
        {
            DialogWindow dialog = new DialogWindow();
            dialog.SetContent(model);

            dialog.Show();
            return true;
        }

        public bool ShowModalWindow(IDialogContent model)
        {
            return ShowModalWindow(model, Application.Current.MainWindow);
        }

        public bool ShowModalWindow(IDialogContent model, Window parentWindow)
        {
            try
            {
                DialogWindow dialog = new DialogWindow
                {
                    Owner = parentWindow,
                };
                dialog.SetContent(model);

                //KeyBinding helpKeyBinding = new KeyBinding(ApplicationHelp.Current.HelpCommand, new KeyGesture(Key.F1));
                //dialog.InputBindings.Add(helpKeyBinding);

                //if (model is IHelpContent)
                //{
                //    dialog.SetContextHelpCommand(ApplicationHelp.Current.ContextHelpCommand, ((IHelpContent) model).HelpTopicId);
                //}

                bool? result = dialog.ShowDialog();
                if (result == null)
                {
                    //throw new Exception(Errors.ResultCannotBeNull); // TODO: create exception
                }

                return (bool) result;
            }
            catch
            {
                //Logger.Error("Could not open modal dialog", ex);
                throw;
            }
        }
    }
}