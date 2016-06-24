namespace TimePlannerNinject.ViewModel
{
    using System;
    using System.Linq;
    using System.Windows.Forms.Integration;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Microsoft.Reporting.WinForms;

    using TimePlannerNinject.Kernel;
    using TimePlannerNinject.Model;
    using TimePlannerNinject.Services;

    /// <summary>
    ///     View model de l'aperçu avant impression.
    /// </summary>
    public class PreviewPrintViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Service de données
        /// </summary>
        private readonly ATimePlannerDataService service;

        /// <summary>
        ///     Date de fin d'impression
        /// </summary>
        private DateTime endGenerationDate;

        /// <summary>
        ///     Commande permettant la génération d'un rapport
        /// </summary>
        private RelayCommand generatePrintReportCommand;

        /// <summary>
        ///     Type de rapport à générer
        /// </summary>
        private ReportType reportType;

        /// <summary>
        ///     Date de début d'impression
        /// </summary>
        private DateTime startGenerationDate;

        /// <summary>
        ///     Réceptable de rapport
        /// </summary>
        private WindowsFormsHost viewer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initialise une nouvelle instance de la classe <see cref="ATimePlannerDataService" />.
        /// </summary>
        /// <param name="service">Le service de données.</param>
        public PreviewPrintViewModel(ATimePlannerDataService service)
        {
            this.service = service;
            var displayDateInCalendar = KernelTimePlanner.Get<CalendrierViewModel>().DateEnCours;
            this.startGenerationDate = new DateTime(displayDateInCalendar.Year, 1, 1);
            this.endGenerationDate = new DateTime(displayDateInCalendar.Year, 12, 31);
        }

        #endregion

        #region Enums

        public enum ReportType
        {
            AllGroupByWorkplace
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Obtient ou définit la date de fin d'impression
        /// </summary>
        public DateTime EndGenerationDate
        {
            get
            {
                return this.endGenerationDate;
            }

            set
            {
                this.Set(nameof(this.EndGenerationDate), ref this.endGenerationDate, value);
            }
        }

        /// <summary>
        ///     Obtient la commande permettant la génération d'un rapport
        /// </summary>
        public RelayCommand GeneratePrintReportCommand
            => this.generatePrintReportCommand ?? (this.generatePrintReportCommand = new RelayCommand(this.ExecuteGeneratePrintReportCommand));

        /// <summary>
        ///     Obtient ou définit le type de rapport pour la génération
        /// </summary>
        public ReportType ReportTypeGeneration
        {
            get
            {
                return this.reportType;
            }

            set
            {
                this.Set(nameof(this.ReportTypeGeneration), ref this.reportType, value);
            }
        }

        /// <summary>
        ///     Obtient ou définit la date de début d'impression
        /// </summary>
        public DateTime StartGenerationDate
        {
            get
            {
                return this.startGenerationDate;
            }

            set
            {
                this.Set(nameof(this.StartGenerationDate), ref this.startGenerationDate, value);
            }
        }

        /// <summary>
        ///     Obtient le réceptable de rapport
        /// </summary>
        public WindowsFormsHost Viewer
        {
            get
            {
                return this.viewer;
            }

            set
            {
                this.Set(nameof(this.Viewer), ref this.viewer, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Netoie le reportViewer
        /// </summary>
        public override void Cleanup()
        {
            this.Viewer = null;
            base.Cleanup();
            GC.Collect();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Execute la commande permettant la génération d'un rapport
        /// </summary>
        private void ExecuteGeneratePrintReportCommand()
        {
            WindowsFormsHost windowsFormsHost;
            switch (this.ReportTypeGeneration)
            {
                case ReportType.AllGroupByWorkplace:
                    windowsFormsHost = new WindowsFormsHost { Child = this.GetReportByWorkplaces() };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(this.ReportTypeGeneration), this.ReportTypeGeneration, null);
            }

            this.Viewer = windowsFormsHost;
        }

        /// <summary>
        ///     Obtient le rapport grouper par lieu de travail
        /// </summary>
        /// <returns>
        ///     Le rapport généré
        /// </returns>
        private ReportViewer GetReportByWorkplaces()
        {
            var reportViewer = new ReportViewer();
            var reportDataSource = new ReportDataSource();
            reportViewer.LocalReport.ReportEmbeddedResource = "TimePlannerNinject.Reports.PrintByWorkPlaces.rdlc";

            reportDataSource.Name = "DataSetReport";
            reportDataSource.Value = (from i in this.service.AllDays
                                      join w in this.service.AllPlaces on i.IdWorkPlace equals w.Id
                                      where
                                          i.WorkStartTime.HasValue && i.WorkStartTime.Value.Date >= this.StartGenerationDate.Date
                                          && i.WorkStartTime.Value.Date <= this.EndGenerationDate.Date
                                      orderby i.WorkStartTime
                                      select new ReportingData(i, w)).ToList();
            ReportParameter[] parameters =
                {
                    new ReportParameter(
                        "Title", 
                        $"Période du {this.StartGenerationDate.Date} au {this.EndGenerationDate.Date}")
                };
            reportViewer.LocalReport.SetParameters(parameters);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);

            reportViewer.RefreshReport();

            return reportViewer;
        }

        #endregion
    }
}