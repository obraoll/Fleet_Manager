using System;
using System.Text.RegularExpressions;

namespace FleetManager.Helpers
{
    /// <summary>
    /// Classe d'aide pour la validation des données
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Valide une adresse email
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Valide une immatriculation française
        /// </summary>
        public static bool IsValidRegistrationNumber(string registration)
        {
            if (string.IsNullOrWhiteSpace(registration))
                return false;

            // Format: AA-123-AA ou 1234-AB-12
            var regex = new Regex(@"^[A-Z]{2}-\d{3}-[A-Z]{2}$|^\d{4}-[A-Z]{2}-\d{2}$");
            return regex.IsMatch(registration.ToUpper());
        }

        /// <summary>
        /// Valide un numéro VIN (17 caractères)
        /// </summary>
        public static bool IsValidVIN(string vin)
        {
            if (string.IsNullOrWhiteSpace(vin))
                return true; // VIN optionnel

            return vin.Length == 17 && Regex.IsMatch(vin, @"^[A-HJ-NPR-Z0-9]{17}$");
        }

        /// <summary>
        /// Valide un numéro de téléphone
        /// </summary>
        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return true; // Téléphone optionnel

            var regex = new Regex(@"^(\+33|0)[1-9](\d{2}){4}$");
            return regex.IsMatch(phone.Replace(" ", "").Replace(".", ""));
        }

        /// <summary>
        /// Valide qu'une chaîne n'est pas vide
        /// </summary>
        public static bool IsNotEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Valide qu'un nombre est positif
        /// </summary>
        public static bool IsPositive(decimal value)
        {
            return value > 0;
        }

        /// <summary>
        /// Valide qu'une année est valide
        /// </summary>
        public static bool IsValidYear(int year)
        {
            return year >= 1900 && year <= DateTime.Now.Year + 1;
        }

        /// <summary>
        /// Valide qu'une date n'est pas dans le futur
        /// </summary>
        public static bool IsNotFutureDate(DateTime date)
        {
            return date <= DateTime.Now;
        }

        /// <summary>
        /// Valide la force d'un mot de passe
        /// </summary>
        public static (bool IsValid, string Message) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Le mot de passe ne peut pas être vide.");

            if (password.Length < 6)
                return (false, "Le mot de passe doit contenir au moins 6 caractères.");

            return (true, "Mot de passe valide.");
        }
    }
}