using SYNKROAPP.CLASES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYNKROAPP.DAO
{
    public interface IDAO
    {
        //USUARIS:
        Task AddUsuari(Usuaris unUsuari);
        Task UpdateUsuari(Usuaris unUsuari);
        Task<Usuaris> GetUsuariByEmail(string targetEmail);
        Task DeleteUsuari(Usuaris unUsuari);

        //EMPRESA:
        Task RegistrarUsuariAmbEmpresa(Usuaris usuari, Empreses empresa);
        Task AddEmpresa(Usuaris unaEmpresa);
        Task UpdateEmpresa(Empreses unaEmpresa);
        Task DeleteEmpresa(Empreses unaEmpresa);

        //MAGATZEMS:
        //Task AddAlmacen(Usuaris unaEmpresa);


    }
}
