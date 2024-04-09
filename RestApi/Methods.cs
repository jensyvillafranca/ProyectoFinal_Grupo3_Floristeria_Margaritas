using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi{
    public static class Methods{
        public static async Task<string> insert_update_async(object data_object,string url) {
            string response_insert = null;

            using(HttpClient client = CreateHttpClient()) {
                var json="";

                if(data_object is TableJoinQueries data){
                    json=JsonConvert.SerializeObject(data);
                } else {
                    /*if(data_object is Table_records record) {
                        json=JsonConvert.SerializeObject(record);
                    } else {
                        if(data_object is Table_services service) {
                            json=JsonConvert.SerializeObject(service);
                        }
                    }*/
                }

                if(json!="") {
                    try {
                        var content = new StringContent(json,Encoding.UTF8,"application/json");

                        var response = await client.PostAsync(url,content);

                        if(response.IsSuccessStatusCode) {
                            response_insert="exitoso";
                        } else {
                            response_insert="error";
                        }
                    } catch(Exception ex) {
                        response_insert=ex+"";
                    }

                } else {
                    response_insert="JSON null";
                }
            }

            return response_insert;
        }

        public static async Task<string> select_async(object data_object,string url){
            string json_response = "";
            string json = "";

            using(HttpClient client = CreateHttpClient()) {
                try {
                    if(data_object is TableJoinQueries data) {
                        json=JsonConvert.SerializeObject(data);
                    } else {
                        /*if(data is Table_records record) {
                            json=JsonConvert.SerializeObject(record);
                        } else {
                            if(data is Table_services service) {
                                json=JsonConvert.SerializeObject(service);
                            } else {
                                if(data is Table_join_data join_data) {
                                    json=JsonConvert.SerializeObject(join_data);
                                }
                            }
                        }*/
                    }

                    var json_content = new StringContent(json,Encoding.UTF8,"application/json");
                    var response = await client.PostAsync(url,json_content);

                    if(response.IsSuccessStatusCode) {
                        json_response=await response.Content.ReadAsStringAsync();
                    } else {
                        json_response="error";
                    }
                } catch(Exception ex) {
                    json_response=ex.ToString();
                }
            }

            return json_response;
        }

        public static HttpClient CreateHttpClient() {
            // Create a handler that ignores SSL certificate errors
            var handler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback=(sender,cert,chain,sslPolicyErrors) => true
            };

            // Create HttpClient with the custom handler
            var httpClient = new HttpClient(handler);

            return httpClient;
        }
    }
    
}
