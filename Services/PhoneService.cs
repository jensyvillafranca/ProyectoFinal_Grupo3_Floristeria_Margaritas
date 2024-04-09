namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Services {
    public static class PhoneService {
        public static async Task make_phone_call(string phone_number){
            try {
                if(PhoneDialer.IsSupported){
                     PhoneDialer.Open(phone_number);
                } else{
                    Console.WriteLine("Hacer llamadas no es soportado en esta plataforma.");
                }
            } catch(ArgumentNullException anEx) {
                // Número de teléfono no proporcionado
                Console.WriteLine(anEx);
            } catch(FeatureNotSupportedException ex) {
                // Llamadas no soportadas en este dispositivo
                Console.WriteLine(ex);
            } catch(Exception ex) {
                // Otras posibles excepciones que se podrían lanzar (por ejemplo, permisos no otorgados)
                Console.WriteLine(ex);
            }
        }
    }
}
