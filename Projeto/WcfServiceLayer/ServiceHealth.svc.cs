using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace WcfServiceLayer
{

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ServiceHealth : IServiceHealth
    {
      

        public bool AddPatient(Patient utente)
        {

            ModelMedAcContainer context = new ModelMedAcContainer();


            Utente ut = new Utente();

            ut.Nome = utente.FirstName;
            ut.Apelido = utente.LastName;
            ut.DataNasc = utente.Birthdate;
            ut.SNS = utente.SNS;
            context.Utentes.Add(ut);
            context.SaveChanges();
            return true;
        }






        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }





        public bool AlterPatient(Patient utente)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();

            Utente ut = context.Utentes.Where(i => i.SNS == utente.oldSNS).FirstOrDefault();
            ut.Nome = utente.FirstName;
            ut.Apelido = utente.LastName;
            ut.DataNasc = utente.Birthdate;
            ut.SNS = utente.SNS;
            ut.Activo = utente.Activo;
            context.SaveChanges();
            return true;
        }

        public bool ActivatePatient(Patient utente)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();

            Utente ut = context.Utentes.Where(i => i.SNS == utente.SNS).FirstOrDefault();
            ut.Activo = utente.Activo;
            context.SaveChanges();
            return true;
        }



        public Patient ValidatePatient(int id)
        {
            //ir a base de dados buscar o utente
            //copiar dados do utente para o patient

            ModelMedAcContainer context = new ModelMedAcContainer();

            Utente ut = context.Utentes.Where(i => i.SNS == id).FirstOrDefault();

            if (ut != null)
            {

                Patient p = new WcfServiceLayer.Patient();
                p.FirstName = ut.Nome;
                p.LastName = ut.Apelido;
                p.SNS = ut.SNS;
                p.Birthdate = Convert.ToDateTime(ut.DataNasc);
                //p.Age = 20;
                p.Age = Calculateage(p.Birthdate);
                p.Activo = ut.Activo;
                System.Console.WriteLine(p);
                return p;
            }
            else return null;

        }


        public static int Calculateage(DateTime BirthDate)
        {
            int age = DateTime.Now.Year - BirthDate.Year;

            if (DateTime.Now.Month < BirthDate.Month || (DateTime.Now.Month == BirthDate.Month
                && DateTime.Now.Day < BirthDate.Day))
            {
                age--;
            }

            return age;
        }

        public bool AddValuesBP(BPs values)
        {

            ModelMedAcContainer context = new WcfServiceLayer.ModelMedAcContainer();

            Utente ut = context.Utentes.Where(i => i.SNS == values.SNS).FirstOrDefault();
            if (ut == null)
                return false;
            else
            {



                BP bp = new WcfServiceLayer.BP();
                bp.Valor1 = values.Value1;
                bp.Valor2 = values.Value2;
                bp.Data = values.Date;
                bp.Alerta = values.Alert;

                bp.Utente = ut;


                context.BPs.Add(bp);
                context.SaveChanges();
                return true;

            }


        }

        public bool AddValuesSPO(SPOes values)
        {
            ModelMedAcContainer context = new WcfServiceLayer.ModelMedAcContainer();

            Utente ut = context.Utentes.Where(i => i.SNS == values.SNS).FirstOrDefault();



            SPO spo = new WcfServiceLayer.SPO();
            spo.Valor = values.Value;
            spo.Data = values.Date;
            spo.Alerta = values.Alert;
            spo.Utente = ut;



            context.SPOes.Add(spo);
            context.SaveChanges();
            return true;

        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool AddValuesHR(HRs values)
        {
            ModelMedAcContainer context = new WcfServiceLayer.ModelMedAcContainer();

            Utente ut = context.Utentes.Where(i => i.SNS == values.SNS).FirstOrDefault();

            HR hr = new WcfServiceLayer.HR();
            hr.Valor = values.Value;
            hr.Data = values.Date;
            hr.Alerta = values.Alert;
            hr.Utente = ut;


            context.HRs.Add(hr);
            context.SaveChanges();
            return true;

        }

        public List<Patient> listActivePatient()
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<Patient> pat = new List<Patient>();
            List<Utente> utBD = context.Utentes.Where(i => i.Activo == true).ToList();


            //copiar cada Utente para Patiente e adicionar a lista de patients
            foreach (Utente item in utBD)
            {
                Patient pt = new Patient();
                pt.Activo = item.Activo;
                pt.SNS = item.SNS;
                pt.Birthdate = Convert.ToDateTime(item.DataNasc);
                pt.Age = Calculateage(pt.Birthdate);
                pt.FirstName = item.Nome;
                pt.LastName = item.Apelido;
                pat.Add(pt);

            }

            return pat;

        }

        public List<Patient> listPatient()
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<Patient> patN = new List<Patient>();
            List<Utente> utBD = context.Utentes.ToList();

            //copiar cada Utente para Patiente e adicionar a lista de patients
            foreach (Utente item in utBD)
            {
                Patient pt = new Patient();
                pt.Activo = item.Activo;
                pt.SNS = item.SNS;
                pt.FirstName = item.Nome;
                pt.LastName = item.Apelido;
                pt.Birthdate = Convert.ToDateTime(item.DataNasc);
                pt.Age = Calculateage(pt.Birthdate);
                patN.Add(pt);

            }

            return patN;

        }

        public List<BPs> listPatientWithAlertsBP()
        {

            ModelMedAcContainer context = new ModelMedAcContainer();
            List<BPs> bpp = new List<BPs>();
            List<BP> bpss = context.BPs.Where(i => i.Alerta == true).ToList();


            foreach (BP item in bpss)
            {
                BPs bpssa = new BPs();

                bpssa.SNS = item.Utente.SNS;


                bpssa.FirstName = item.Utente.Nome;
                bpssa.LastName = item.Utente.Apelido;
                bpssa.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                bpssa.Age = Calculateage(bpssa.Birthdate);
                bpssa.Value1 = item.Valor1;
                bpssa.Value2 = item.Valor2;
                bpssa.Date = item.Data;
                bpp.Add(bpssa);

            }
            return bpp;
        }

        public List<HRs> listPatientWithAlertsHR()
        {

            ModelMedAcContainer context = new ModelMedAcContainer();
            List<HRs> hrr = new List<HRs>();
            List<HR> hrss = context.HRs.Where(i => i.Alerta == true).ToList();




            foreach (HR item in hrss)
            {
                HRs hrssa = new HRs();

                hrssa.SNS = item.Utente.SNS;




                hrssa.FirstName = item.Utente.Nome;
                hrssa.LastName = item.Utente.Apelido;
                hrssa.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                hrssa.Age = Calculateage(hrssa.Birthdate);
                hrssa.Value = item.Valor;
                hrssa.Date = item.Data;
                hrr.Add(hrssa);

            }
            return hrr;
        }

        public List<SPOes> listPatientWithAlertsSPO()
        {

            ModelMedAcContainer context = new ModelMedAcContainer();
            List<SPOes> spp = new List<SPOes>();
            List<SPO> spss = context.SPOes.Where(i => i.Alerta == true).ToList();




            foreach (SPO item in spss)
            {
                SPOes sposse = new SPOes();

                sposse.SNS = item.Utente.SNS;




                sposse.FirstName = item.Utente.Nome;
                sposse.LastName = item.Utente.Apelido;
                sposse.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                sposse.Age = Calculateage(sposse.Birthdate);
                sposse.Value = item.Valor;
                sposse.Date = item.Data;
                spp.Add(sposse);

            }
            return spp;
        }


        public List<BPs> listBPs(int sns, DateTime di, DateTime df)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<BPs> bpp = new List<BPs>();
            List<BP> bpss = context.BPs.Where(i => i.Utente.SNS == sns && i.Data >= di && i.Data <= df).ToList();
            foreach (BP item in bpss)
            {
                BPs bpssa = new BPs();

                bpssa.Value1 = item.Valor1;
                bpssa.Value2 = item.Valor2;
                bpssa.Date = item.Data;
                bpssa.SNS = item.Utente.SNS;
                bpssa.FirstName = item.Utente.Nome;
                bpssa.LastName = item.Utente.Apelido;
                bpssa.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                bpssa.Age = Calculateage(bpssa.Birthdate);
                bpp.Add(bpssa);


            }
            return bpp;
        }

            public List<Alertas> listAlertas(DateTime di, DateTime df)
            {
                ModelMedAcContainer context = new ModelMedAcContainer();
                List<Alertas> all = new List<Alertas>();
                List<Alerta> allt = context.Alertas.Where(i => i.Data >= di && i.Data <= df).ToList();
                foreach (Alerta item in allt)
                {
                    Alertas allsa = new Alertas();

                    allsa.Alert = item.Parametro;
                    allsa.Tipo = item.Tipo;
                    allsa.Date = item.Data;
                    allsa.SNS = item.Utente.SNS;
                    allsa.FirstName = item.Utente.Nome;
                    allsa.LastName = item.Utente.Apelido;
                
                    all.Add(allsa);


                }
                return all;

            }

        public List<Alertas> listAlertasUtente(int sns, DateTime di, DateTime df)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<Alertas> all = new List<Alertas>();
            List<Alerta> allt = context.Alertas.Where(i => i.Utente.SNS == sns && i.Data >= di && i.Data <= df).ToList();
            foreach (Alerta item in allt)
            {
                Alertas allsa = new Alertas();

                allsa.Alert = item.Parametro;
                allsa.Tipo = item.Tipo;
                allsa.Date = item.Data;
                allsa.SNS = item.Utente.SNS;
                allsa.FirstName = item.Utente.Nome;
                allsa.LastName = item.Utente.Apelido;

                all.Add(allsa);


            }
            return all;

        }

        public List<SPOes> listSPOes(int sns, DateTime di, DateTime df)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<SPOes> spp = new List<SPOes>();
            List<SPO> spss = context.SPOes.Where(i => i.Utente.SNS == sns && i.Data >= di && i.Data <= df).ToList();
            foreach (SPO item in spss)
            {
                SPOes spsso = new SPOes();

                spsso.Value = item.Valor;
                spsso.Date = item.Data;
                spsso.SNS = item.Utente.SNS;
                spsso.FirstName = item.Utente.Nome;
                spsso.LastName = item.Utente.Apelido;
                spsso.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                spsso.Age = Calculateage(spsso.Birthdate);
                spp.Add(spsso);


            }
            return spp;
        }
        public List<HRs> listHRs(int sns, DateTime di, DateTime df)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<HRs> hrr = new List<HRs>();
            List<HR> hrss = context.HRs.Where(i => i.Utente.SNS == sns && i.Data >= di && i.Data <= df).ToList();
            foreach (HR item in hrss)
            {
                HRs hrsso = new HRs();

                hrsso.Value = item.Valor;
                hrsso.Date = item.Data;
                hrsso.SNS = item.Utente.SNS;
                hrr.Add(hrsso);


            }
            return hrr;
        }
        public List<BPs> listAlertsBPs(DateTime di, DateTime df)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<BPs> bpp = new List<BPs>();
            List<BP> bpps = context.BPs.Where(i => i.Data >= di && i.Data <= df && i.Valor2 < 90 || i.Valor1 > 180).ToList();

            foreach (BP item in bpps)
            {


                BPs bpssa = new BPs();

                bpssa.Value1 = item.Valor1;
                bpssa.Value2 = item.Valor2;
                bpssa.Date = item.Data;
                bpssa.SNS = item.Utente.SNS;
                bpssa.FirstName = item.Utente.Nome;
                bpssa.LastName = item.Utente.Apelido;
                bpssa.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                bpssa.Age = Calculateage(bpssa.Birthdate);
                bpp.Add(bpssa);

            }
            return bpp;
        }

        public List<HRs> listAlertsHRs(DateTime di, DateTime df)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<HRs> hrr = new List<HRs>();
            List<HR> hrss = context.HRs.Where(i => i.Data >= di && i.Data <= df && i.Valor < 60 || i.Valor > 120).ToList();
            foreach (HR item in hrss)
            {
                HRs hrsso = new HRs();

                hrsso.Value = item.Valor;
                hrsso.Date = item.Data;
                hrsso.SNS = item.Utente.SNS;
                hrsso.FirstName = item.Utente.Nome;
                hrsso.LastName = item.Utente.Apelido;
                hrsso.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                hrsso.Age = Calculateage(hrsso.Birthdate);
                hrr.Add(hrsso);


            }
            return hrr;
        }

        public List<SPOes> listAlertsSPOes(DateTime di, DateTime df)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<SPOes> spp = new List<SPOes>();
            List<SPO> spss = context.SPOes.Where(i => i.Data >= di && i.Data <= df && i.Valor < 90).ToList();
            foreach (SPO item in spss)
            {
                SPOes spsso = new SPOes();

                spsso.Value = item.Valor;
                spsso.Date = item.Data;
                spsso.SNS = item.Utente.SNS;
                spsso.FirstName = item.Utente.Nome;
                spsso.LastName = item.Utente.Apelido;
                spsso.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                spsso.Age = Calculateage(spsso.Birthdate);
                spp.Add(spsso);


            }
            return spp;
        }

        public List<BPs> listUserAlertsBPs(int sns, DateTime di, DateTime df)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<BPs> bpp = new List<BPs>();
            List<BP> bpps = context.BPs.Where(i => i.Utente.SNS == sns && i.Data >= di && i.Data <= df).ToList();

            foreach (BP item in bpps)
            {
                BPs bpssa = new BPs();

                bpssa.Value1 = item.Valor1;
                bpssa.Value2 = item.Valor2;
                bpssa.Date = item.Data;
                bpssa.SNS = item.Utente.SNS;
                bpssa.FirstName = item.Utente.Nome;
                bpssa.LastName = item.Utente.Apelido;
                bpssa.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                bpssa.Age = Calculateage(bpssa.Birthdate);
                bpp.Add(bpssa);

            }
            return bpp;
        }

        public List<HRs> listUserAlertsHRs(int sns, DateTime di, DateTime df)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<HRs> hrr = new List<HRs>();
            List<HR> hrss = context.HRs.Where(i => i.Utente.SNS == sns && i.Data >= di && i.Data <= df).ToList();
            foreach (HR item in hrss)
            {
                HRs hrsso = new HRs();

                hrsso.Value = item.Valor;
                hrsso.Date = item.Data;
                hrsso.SNS = item.Utente.SNS;
                hrsso.FirstName = item.Utente.Nome;
                hrsso.LastName = item.Utente.Apelido;
                hrsso.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                hrsso.Age = Calculateage(hrsso.Birthdate);
                hrr.Add(hrsso);


            }
            return hrr;
        }

        public List<SPOes> listUserAlertsSPOes(int sns, DateTime di, DateTime df)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            List<SPOes> spp = new List<SPOes>();
            List<SPO> spss = context.SPOes.Where(i => i.Utente.SNS == sns && i.Data >= di && i.Data <= df).ToList();
            foreach (SPO item in spss)
            {
                SPOes spsso = new SPOes();

                spsso.Value = item.Valor;
                spsso.Date = item.Data;
                spsso.SNS = item.Utente.SNS;
                spsso.FirstName = item.Utente.Nome;
                spsso.LastName = item.Utente.Apelido;
                spsso.Birthdate = Convert.ToDateTime(item.Utente.DataNasc);
                spsso.Age = Calculateage(spsso.Birthdate);
                spp.Add(spsso);


            }
            return spp;
        }

        public bool AddAlerts(Alertas valores)
        {
            ModelMedAcContainer context = new ModelMedAcContainer();
            Utente ut = context.Utentes.Where(i => i.SNS == valores.SNS).FirstOrDefault();
            Alerta alt = new WcfServiceLayer.Alerta();
            alt.Data = valores.Date;
            alt.Parametro = valores.Alert;
            alt.Tipo = valores.Tipo; 
            alt.Utente = ut;
            context.Alertas.Add(alt);
            context.SaveChanges();
            return true; 
        }
    }
}

 