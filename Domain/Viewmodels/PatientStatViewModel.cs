namespace Domain.Viewmodels
{
    public class PatientStatViewModel
    {

        public int Patients { get; set; }
        public int Males { get; set; }
        public int Females { get; set; }
        public int Others { get; set; }


        public PatientStatViewModel(int one, int two, int three, int four)
        {

            Patients = one;
            Males = two;
            Females = three;
            Others = four;
        }

    }
}
