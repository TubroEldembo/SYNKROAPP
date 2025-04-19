using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using SYNKROAPP.CLASES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SYNKROAPP.DAO
{
    public class DAOImpl: IDAO
    {
        FirestoreDb db;
        private string token;
        private string projectName;

        public DAOImpl(string token, string projectName)
        {
            this.token = token;
            this.projectName = projectName;
            SetCredentials();
            db = FirestoreDb.Create(projectName);
        }

        private void SetCredentials()
        {
            string pathToServiceAccount = "credencials.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToServiceAccount);
        }

        public async Task RegistrarUsuariAmbEmpresa(Usuaris unUsuari, Empreses unaEmpresa)
        {
            try
            {
                if (unUsuari == null)
                    throw new Exception("L'usuari no és vàlid.");

                if (unaEmpresa == null)
                    throw new Exception("L'empresa no és vàlida.");

                // 1. Crear documento empresa con ID generado manualmente
                string empresaDocId = Guid.NewGuid().ToString();
                unaEmpresa.EmpresaID = empresaDocId;

                DocumentReference empresaRef = db.Collection("Empreses").Document(empresaDocId);
                await empresaRef.SetAsync(unaEmpresa);

                // 2. Crear documento usuario con ID generado manualmente
                string usuariDocId = Guid.NewGuid().ToString();
                unUsuari.UsuariID = usuariDocId;
                unUsuari.EmpresaID = empresaDocId;

                DocumentReference usuariRef = db.Collection("Usuaris").Document(usuariDocId);
                await usuariRef.SetAsync(unUsuari);

                // 3. Añadir el ID del usuario a la lista Usuaris de la empresa
                await empresaRef.UpdateAsync("Usuaris", FieldValue.ArrayUnion(usuariDocId));

                MessageBox.Show("Usuari i empresa registrats correctament!", "Èxit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durant el registre: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public Task AddUsuari(Usuaris unUsuari)
        {
            throw new NotImplementedException();
        }
        public Task AddEmpresa(Usuaris unaEmpresa)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEmpresa(Empreses unaEmpresa)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUsuari(Usuaris unUsuari)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuaris> GetUsuariByEmail(string targetEmail)
        {
            CollectionReference colRef = db.Collection("Usuaris");
            Query query = colRef.WhereEqualTo("Email", targetEmail);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (!snapshot.Documents.Any())
            {
                MessageBox.Show($"No se encontró ningún usuario con el correo: {targetEmail}");
                return null;
            }

            DocumentSnapshot doc = snapshot.Documents.First();

            Console.WriteLine($"Documento encontrado: {doc.Id}");
            foreach (var kv in doc.ToDictionary())
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }

            Usuaris usuario = doc.ConvertTo<Usuaris>();
            return usuario;
        }

        public Task UpdateEmpresa(Empreses unaEmpresa)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUsuari(Usuaris unUsuari)
        {
            throw new NotImplementedException();
        }

    }
}
