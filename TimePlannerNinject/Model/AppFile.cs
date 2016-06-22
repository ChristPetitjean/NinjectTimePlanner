﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppFile.cs" company="Christophe PETITJEAN">
//   Christophe PETITJEAN - 2016
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TimePlannerNinject.Model
{
    using System;

    /// <summary>
    ///     Décrit un fichier de sauvegarde.
    /// </summary>
    [Serializable]
    public class AppFile
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="AppFile" />.
        /// </summary>
        public AppFile()
        {
            this.Inputdays = new InputDay[0];
            this.Worplaces = new WorkPlace[0];
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Obtient ou définit les inputations.
        /// </summary>
        public InputDay[] Inputdays { get; set; }

        /// <summary>
        ///     Obtient ou définit les lieux de travails.
        /// </summary>
        public WorkPlace[] Worplaces { get; set; }

        #endregion
    }
}