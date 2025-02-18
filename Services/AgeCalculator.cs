using System;

public class AgeCalculator {
    public static int getAge(int year, int month, int day) {
        try {
            DateTime birthDate = new DateTime(year, month, day);
            DateTime today = DateTime.Today;

            int age = today.Year - birthDate.Year;
            if((today.Month < birthDate.Month) ||
            (today.Month == birthDate.Month &&
            today.Day < birthDate.Day)) {
                age--;
            }
            
            return age;
        }
        catch {
            return -1;
        }
    }
}