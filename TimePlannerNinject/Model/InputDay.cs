// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputDay.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Model
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    using GalaSoft.MvvmLight;

    /// <summary>
    ///     The input day.
    /// </summary>
    [Serializable]
    public class InputDay : ObservableObject, ISerializable, ICloneable
    {
        #region Fields

        /// <summary>
        ///     Heures supplémentaires.
        /// </summary>
        private int? extraHours;

        /// <summary>
        ///     Identifiant de l'événement.
        /// </summary>
        private int id;

        /// <summary>
        ///     Lieux de travail.
        /// </summary>
        private int? idWorkPlace;

        /// <summary>
        ///     Heure de fin du travail quotidien.
        /// </summary>
        private DateTime? workEndTime;

        /// <summary>
        ///     Heure de début du travail quotidien.
        /// </summary>
        private DateTime? workStartTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="InputDay" />.
        /// </summary>
        public InputDay()
        {
        }

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="InputDay" />.
        /// </summary>
        /// <param name="info">
        ///     Les infos de sérilisation.
        /// </param>
        /// <param name="context">
        ///     le context de sérialisation.
        /// </param>
        protected InputDay(SerializationInfo info, StreamingContext context)
        {
            this.ExtraHours = (int?)info.GetValue("ExtraHours", typeof(int?));
            this.WorkEndTime = (DateTime?)info.GetValue("WorkEndTime", typeof(DateTime?));
            this.WorkStartTime = (DateTime?)info.GetValue("WorkStartTime", typeof(DateTime?));
            this.IdWorkPlace = (int?)info.GetValue("IdWorkPlace", typeof(int?));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Obtient ou définit les heures supplémentaires.
        /// </summary>
        public int? ExtraHours
        {
            get
            {
                return this.extraHours;
            }

            set
            {
                this.Set(nameof(this.ExtraHours), ref this.extraHours, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit l'identifiant.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int ID
        {
            get
            {
                return this.id;
            }

            set
            {
                this.Set(nameof(this.ID), ref this.id, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit le lieux de travail pour l'entrée.
        /// </summary>
        public int? IdWorkPlace
        {
            get
            {
                return this.idWorkPlace;
            }

            set
            {
                this.Set(nameof(this.IdWorkPlace), ref this.idWorkPlace, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit l'heure de fin du travail quotidien.
        /// </summary>
        public DateTime? WorkEndTime
        {
            get
            {
                return this.workEndTime;
            }

            set
            {
                this.Set(nameof(this.WorkEndTime), ref this.workEndTime, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit l'heure de début du travail quotidien.
        /// </summary>
        public DateTime? WorkStartTime
        {
            get
            {
                return this.workStartTime;
            }

            set
            {
                this.Set(nameof(this.WorkStartTime), ref this.workStartTime, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Crée un objet qui est une copie de l'instance actuelle.
        /// </summary>
        /// <returns>
        ///     Nouvel objet qui est une copie de cette instance.
        /// </returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

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
            info.AddValue("ExtraHours", this.ExtraHours);
            info.AddValue("WorkEndTime", this.WorkEndTime);
            info.AddValue("WorkStartTime", this.WorkStartTime);
            info.AddValue("IdWorkPlace", this.IdWorkPlace);
        }

        #endregion
    }
}