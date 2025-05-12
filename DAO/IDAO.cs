using Google.Cloud.Firestore;
using SYNKROAPP.CLASES;
using SYNKROAPP.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

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
        Task<List<ZonaProductoViewModel>> ObtenerZonasDeProducto(string producteID);

        //AGREGAR PRODUCTS:
        Task<DocumentSnapshot> GetProducteGeneralPorID(string producteID);

        Task GuardarMovimientoInventariAsync(MovimentsInventari movimentTraslado, ProductesInventari inventari, bool productAlreadyExists);

        Task AddInventariToZona(ProductesInventari inventari);



        //CARGAR CATEGORIAS Y SUBCATEGORIAS (GENERICAS Y PERSONALIZADAS):
        Task<(List<CategoriaGenerica>, Dictionary<string, List<SubcategoriaGenerica>>)> ObtenirCategoriesGeneriques();

        // Cargar categorías y subcategorías personalizadas de una empresa
        Task<(List<Categories>, Dictionary<string, List<Subcategories>>)> ObtenirCategoriesPersonalitzades(string empresaID);


        // VISTA RESUMEN (HOME)
        Task<List<Magatzems>> ObtenerLosAlmacenesTotalesDeLaEmpresa(string empresaID);
        Task<Empreses> GetEmpresaByID(string empresaID);
        Task<Dictionary<string, int>> ObtenerProductosEnVentaPorCategoria(string empresaID);

        // STORAGE 
        Task<string> StoreImage(string localPath, string nameToStore);
        BitmapImage LoadImageFromUrl(string url);

        // GET
        Task<DocumentSnapshot> GetProducteInventariPorID(string producteID);
        Task<List<MovimentsInventari>> ObtenerMovimientosInventarioPorEmpresa(string empresaID);
        Task<List<string>> GetEmpresesAmbProductesEnVenda();
        Task<List<ProducteAmbDetall>> GetProductosCatagalogoD1Empresa(string empresaID, bool soloEnVenta);

        //POST
        Task AddProductoGeneral(ProducteGeneral producto, DetallProducte detall);
        Task<ProductesInventari> GetProductoInventario(string empresaID, string magatzemPertanyent, string zonaEmmagatzematgeID, string inventariID);
        Task<string> CheckProductInZona(string productoID, string empresaID, string magazemID, string zonaID);

        #region IMPORTAR PRODUCTOS
        List<ProducteAmbDetall> LlegeixProductesJson(string filePath);
        #endregion
    }
}
