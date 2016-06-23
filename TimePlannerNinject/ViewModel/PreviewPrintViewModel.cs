namespace TimePlannerNinject.ViewModel
{
    using System;
    using System.Linq;
    using System.Windows.Forms.Integration;

    using GalaSoft.MvvmLight;

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
            var windowsFormsHost = new WindowsFormsHost { Child = this.GetReportByWorkplaces() };
            this.viewer = windowsFormsHost;
        }

        #endregion

        #region Public Properties

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

        #region Methods

        /// <summary>
        /// Obtient le rapport grouper par lieu de travail
        /// </summary>
        /// <returns>
        /// Le rapport généré
        /// </returns>
        private ReportViewer GetReportByWorkplaces()
        {
            var reportViewer = new ReportViewer();
            var reportDataSource = new ReportDataSource();
            reportViewer.LocalReport.ReportEmbeddedResource = "TimePlannerNinject.Reports.PrintByWorkPlaces.rdlc";

            DateTime currentDate = KernelTimePlanner.Get<CalendrierViewModel>().DateEnCours;
            reportDataSource.Name = "DataSetReport";
            reportDataSource.Value = (from i in this.service.AllDays
                                      join w in this.service.AllPlaces on i.IdWorkPlace equals w.Id
                                      where
                                          i.WorkStartTime.HasValue && i.WorkStartTime.Value.Year == currentDate.Year && i.WorkEndTime.HasValue
                                          && i.WorkEndTime.Value.Year == currentDate.Year
                                      orderby i.WorkStartTime
                                      select new ReportingData(i, w)).ToList();
            ReportParameter[] parameters =
                {
                    new ReportParameter(
                        "Title", 
                        $"Période du 1er janvier {currentDate.Year} au 31 décembre {currentDate.Year}")
                };
            reportViewer.LocalReport.SetParameters(parameters);
            reportViewer.LocalReport.DataSources.Add(reportDataSource);

            reportViewer.RefreshReport();

            return reportViewer;
        }

        #endregion

        /// <summary>
        /// Netoie le reportViewer
        /// </summary>
        public override void Cleanup()
        {
            this.Viewer = null;
            base.Cleanup();
            GC.Collect();
        }
    }
}