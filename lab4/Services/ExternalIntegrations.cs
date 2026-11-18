using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Lab4_University.Services
{
    public interface IEdeboRegistryService
    {
        bool ValidateCardFormatAndRegister(string cardNumber);
    }

    public class EdeboRegistryStub : IEdeboRegistryService
    {
        private readonly HashSet<string> _registeredCards = new HashSet<string>();

        public bool ValidateCardFormatAndRegister(string cardNumber)
        {
            if (!Regex.IsMatch(cardNumber ?? "", @"^ST-\d{4}$"))
                return false;

            return _registeredCards.Add(cardNumber);
        }
    }

    public interface INotificationService
    {
        void SendNotification(string recipientEmail, string message);
    }

    public class ConsoleNotificationService : INotificationService
    {
        public void SendNotification(string recipientEmail, string message)
        {
            Console.WriteLine($"  [Зовнішній сервіс сповіщень -> {recipientEmail}]: {message}");
        }
    }
}