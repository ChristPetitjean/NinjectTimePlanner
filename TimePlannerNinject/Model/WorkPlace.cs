// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkPlace.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Model
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Media;

    using GalaSoft.MvvmLight;

    using Color = System.Windows.Media.Color;

    /// <summary>
    ///     Définit un lieu de travail.
    /// </summary>
    [Serializable]
    public class WorkPlace : ObservableObject, ISerializable
    {
        #region Fields

        /// <summary>
        /// Formattage des dates pour la sauvegarde
        /// </summary>
        private static string DateFormatter = "dd/MM/y - hh:mm:ss";

        /// <summary>
        ///     Couleur du lieu.
        /// </summary>
        private Color color;

        /// <summary>
        ///     Date de fin du travail quotidien par défaut.
        /// </summary>
        private DateTime defaultEndTime;

        /// <summary>
        ///     date de début du travail quotidien par défaut.
        /// </summary>
        private DateTime defaultStartTime;

        /// <summary>
        ///     Identifiant du lieux de travail
        /// </summary>
        private int id;

        /// <summary>
        ///     Nom du lieu de travail.
        /// </summary>
        private string name;

        /// <summary>
        ///     Kilomètres jusqu'au lieu de travail (aller).
        /// </summary>
        private decimal oneWayKilometers;

        /// <summary>
        ///     Kilomètres jusqu'au lieu de travail (retour).
        /// </summary>
        private decimal returnKilometers;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="WorkPlace" />.
        /// </summary>
        public WorkPlace()
        {
            this.Id = 0;
            this.Name = string.Empty;
            this.DefaultStartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            this.DefaultEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);
            this.Color = Colors.Black;
            this.OneWayKilometers = 0;
        }

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="WorkPlace" />.
        /// </summary>
        /// <param name="info">
        ///     Les infos de sérilisation.
        /// </param>
        /// <param name="context">
        ///     le context de sérialisation.
        /// </param>
        protected WorkPlace(SerializationInfo info, StreamingContext context)
        {
            var colorString = info.GetString("Color");
            var fromHtml = ColorTranslator.FromHtml(colorString);
            this.Color = Color.FromArgb(fromHtml.A, fromHtml.R, fromHtml.G, fromHtml.B);
            this.DefaultEndTime = DateTime.ParseExact(info.GetString("DefaultEndTime"), DateFormatter, CultureInfo.InvariantCulture); 
            this.DefaultStartTime = DateTime.ParseExact(info.GetString("DefaultStartTime"), DateFormatter, CultureInfo.InvariantCulture);
            this.Name = info.GetString("Name");
            this.OneWayKilometers = info.GetDecimal("Km1");
            this.ReturnKilometers = info.GetDecimal("Km2");
            this.Id = info.GetInt32("Id");
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Obtient ou définit la couleur du lieu.
        /// </summary>
        public Color Color
        {
            get
            {
                return this.color;
            }

            set
            {
                this.Set(nameof(this.Color), ref this.color, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit la date de fin du travail quotidien.
        /// </summary>
        public DateTime DefaultEndTime
        {
            get
            {
                return this.defaultEndTime;
            }

            set
            {
                this.Set(nameof(this.DefaultEndTime), ref this.defaultEndTime, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit la date de début du travail quotidien.
        /// </summary>
        public DateTime DefaultStartTime
        {
            get
            {
                return this.defaultStartTime;
            }

            set
            {
                this.Set(nameof(this.DefaultStartTime), ref this.defaultStartTime, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit l'identifiant du lieu de travail.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.Set(nameof(this.Id), ref this.id, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit le nom du lieu de travail.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.Set(nameof(this.Name), ref this.name, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit le nombre de kilometres jusqu'au lieu de travail.
        /// </summary>
        public decimal OneWayKilometers
        {
            get
            {
                return this.oneWayKilometers;
            }

            set
            {
                this.Set(nameof(this.OneWayKilometers), ref this.oneWayKilometers, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit le nombre de kilometres jusqu'au lieu de travail.
        /// </summary>
        public decimal ReturnKilometers
        {
            get
            {
                return this.returnKilometers;
            }

            set
            {
                this.Set(nameof(this.ReturnKilometers), ref this.returnKilometers, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Remplit <see cref="T:System.Runtime.Serialization.SerializationInfo" /> avec les données nécessaires pour
        ///     sérialiser
        ///     l'objet cible.
        /// </summary>
        /// <param name="info">
        ///     <see cref="T:System.Runtime.Serialization.SerializationInfo" /> à remplir de données.
        /// </param>
        /// <param name="context">
        ///     Destination (consultez <see cref="T:System.Runtime.Serialization.StreamingContext" />) de cette
        ///     sérialisation.
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Color", ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(this.Color.A, this.Color.R, this.Color.G, this.Color.B)));
            info.AddValue("DefaultEndTime", this.DefaultEndTime.ToString(DateFormatter));
            info.AddValue("DefaultStartTime", this.DefaultStartTime.ToString(DateFormatter));
            info.AddValue("Name", this.Name);
            info.AddValue("Km1", this.OneWayKilometers);
            info.AddValue("Km2", this.ReturnKilometers);
            info.AddValue("Id", this.Id);
        }

        #endregion
    }
}