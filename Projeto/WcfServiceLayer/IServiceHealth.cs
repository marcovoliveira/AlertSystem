using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfServiceLayer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IServiceHealth
    {

        [OperationContract]
        bool AddPatient(Patient utente);
       

        [OperationContract]
        bool AddValuesBP(BPs values);

        [OperationContract]
        bool AddValuesSPO(SPOes values);

        [OperationContract]
        bool AddValuesHR(HRs values);


        [OperationContract]
        bool AddAlerts(Alertas valores);


        [OperationContract]
        Patient ValidatePatient(int id);

        [OperationContract]
        List<Patient> listActivePatient();

        [OperationContract]
        List<Patient> listPatient();

        
        [OperationContract]
        List<BPs> listPatientWithAlertsBP();

        [OperationContract]
        List<SPOes> listPatientWithAlertsSPO();

        [OperationContract]
        List<HRs> listPatientWithAlertsHR();

        [OperationContract]
        List<Alertas> listAlertas(DateTime di, DateTime df);

        [OperationContract]
        List<Alertas> listAlertasUtente(int sns, DateTime di, DateTime df);

        [OperationContract]
        List<BPs> listBPs(int sns, DateTime di, DateTime df);

        [OperationContract]
        List<SPOes> listSPOes(int sns, DateTime di, DateTime df);

        [OperationContract]
        List<HRs> listHRs(int sns, DateTime di, DateTime df);

        [OperationContract]
        List<BPs> listAlertsBPs(DateTime di, DateTime df);

        [OperationContract]
        List<HRs> listAlertsHRs(DateTime di, DateTime df);

        [OperationContract]
        List<SPOes> listAlertsSPOes(DateTime di, DateTime df);

        [OperationContract]
        List<BPs> listUserAlertsBPs(int sns, DateTime di, DateTime df);

        [OperationContract]
        List<HRs> listUserAlertsHRs(int sns, DateTime di, DateTime df);

        [OperationContract]
        List<SPOes> listUserAlertsSPOes(int sns, DateTime di, DateTime df);

        [OperationContract]
        bool AlterPatient(Patient utente);

        [OperationContract]
        bool ActivatePatient(Patient utente);

       



        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }

    [DataContract]

    public class Patient
    {

        [DataMember]
        public int oldSNS;

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public int PatientID { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int SNS { get; set; }

        [DataMember]
        public DateTime Birthdate { get; set; }


        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public bool Activo { get; set; }

        [DataMember]
        public BP bps { get; set; }

        [DataMember]
        public int Value1 { get; set; }




    }

    [DataContract]
    public class BPs
    {
        [DataMember]
        public  int Value1 { get; set; }

        [DataMember]
        public int Value2 { get; set; }

        [DataMember]
        public int SNS { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public bool Alert { get; set; }

        [DataMember]
        public Patient patient { get; set; }

        [DataMember]
        public int sns { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int Age { get; set; }


        [DataMember]
        public DateTime Birthdate { get; set; }

    }

    [DataContract]
    public class SPOes
    {
        [DataMember]
        public int Value { get; set; }

        [DataMember]
        public int SNS { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public bool Alert { get; set; }

        [DataMember]
        public Patient patient { get; set; }

        [DataMember]
        public int sns { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int Age { get; set; }

    
        [DataMember]
        public DateTime Birthdate { get; set; }

        //public object Value1 { get; internal set; }
    }

    [DataContract]
    public class HRs
    {
        [DataMember]
        public int Value { get; set; }

        [DataMember]
        public int SNS { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public bool Alert { get; set; }

        [DataMember]
        public Patient patient { get; set; }

        [DataMember]
        public int sns { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int Age { get; set; }


        [DataMember]
        public DateTime Birthdate { get; set; }

    }
    
    [DataContract]
    public class Alertas
    {
        [DataMember]
        public int SNS { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public Patient Patient { get; set; }

        [DataMember]
        public BPs Bp { get; set; }

        [DataMember]
        public HRs Hr { get; set; }

        [DataMember]
        public SPOes Spoe { get; set; }

        [DataMember]
        public DateTime Date { get; set; }
        
        [DataMember]
        public string Alert { get; set; }

        [DataMember]
        public string Tipo { get; set; }
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.

    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
