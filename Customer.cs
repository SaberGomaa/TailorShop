namespace TailorShop
{
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }        // الاسم
        public double Length { get; set; }      // الطول
        public double Sleeve { get; set; }      // الكم
        public double Shoulder { get; set; }    // الكتف
        public double Width { get; set; }       // الوسع
        public double Fitness { get; set; }     // اللياقة
        public double Collar { get; set; }      // الطوق
        public string Embroidery { get; set; }  // التطريز
        public string Phone { get; set; }       // التليفون
        public string Notes { get; set; }       // ملاحظات
    }
}