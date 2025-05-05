using Google.Cloud.Firestore;
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
        Task AddEmpresa(Empreses unaEmpresa);
        Task UpdateEmpresa(Empreses unaEmpresa);
        Task DeleteEmpresa(Empreses unaEmpresa);


        //MAGATZEMS:
        Task AddAlmacen(Empreses unaEmpresa, Magatzems unMagatzem);
        Task<Magatzems> CrearAlmacen(Empreses unaEmpresa, Magatzems nouMagatzem);
        Task<List<Magatzems>> DetallesAlmacenes(Empreses empresa);


        //ZONES EMMAGATZEMATGE:
        Task<ZonaEmmagatzematge> CrearZonaAlmacen(Magatzems unMagatzem, ZonaEmmagatzematge novaZona);
        Task<List<ZonaEmmagatzematge>> DetallesZonasAlmacen(Magatzems unMagatzem);
        Task<List<ProductesInventari>> ProductosEn1Zona(ZonaEmmagatzematge zona);


        //AGREGAR PRODUCTS:
        Task <bool> AddProducts2Zone(ProducteGeneral producteGeneral, DetallProducte detallProducte, ProductesInventari inventari);
        Task<DocumentSnapshot> GetProducteGeneralPorID(string producteID);


        //CARGAR CATEGORIAS Y SUBCATEGORIAS (GENERICAS Y PERSONALIZADAS):
        Task<(List<CategoriaGenerica>, Dictionary<string, List<SubcategoriaGenerica>>)> ObtenirCategoriesGeneriques();

        // Cargar categorías y subcategorías personalizadas de una empresa
        Task<(List<Categories>, Dictionary<string, List<Subcategories>>)> ObtenirCategoriesPersonalitzades(string empresaID);


        // VISTA RESUMEN (HOME)
        Task<List<Magatzems>> ObtenerLosAlmacenesTotalesDeLaEmpresa(string empresaID);
        Task<Empreses> GetEmpresaByID(string empresaID);
        Task<Dictionary<string, int>> ObtenerProductosEnVentaPorCategoria(string empresaID);




        
    }
}
