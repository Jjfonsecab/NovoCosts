# NovoCosts

Aplicacion de escritorio hecha en .Net con C# y SQL Server para costear los precios de los muebles en una fabrica. 
Al finalizar el proceso la aplicacion genera un PDF personalizado con la informacion relacionada a un producto seleccionado, 
su coste final, porcentaje de ganancia, utilidad y precio de fabrica.Todo lo anterior acompañado del listado de las partes y 
materiales asignados a cada producto junto con el coste de la mano de obra. 


## Appendice

La base de la aplicaciones un stringConnection que realiza al coneccion a la base de datos, este es usdo posteriormente en la clase DbDatos para quenerar y cerrar la conexion cuando es necesario.

En esta clase existen 2 metodos que son pilares en la aplicacion:
 1. Tenemos el metodo Ejecutar, te tipo bool que se encargara de ejecutar procedure como guardar y ejecutar.
 2. Listar es el segundo metodo y este nos retorna un DataTble

Los metodos nombrados anteriormente utilizan la cclase Paramtro esta se usa dependiendo de las necesidadaes del rpocedure a llamar.

Luego tenemos 9 forms que componen la aplicacion estos van desde el inicio donde se selecciona la ruta en base al proceso que requiera hacer el usuario. Este proceso puede ser desde insertar las materias primas en la aplicaion, registrar el nombre de un producto, costear las partes de un producto, aagregar los costes de la mano de obra hasta generar el pdf con los resultados obtenidos.

## Environment Variables

Este proyecto es un monolito por lo que la unica configuracion necesaria es configurar los datos del StringConnection, para la base de datos que se va a usar. Para esta aplicacion se uso MySql.

##Componentes del proyecto:

1. La base del proyecto son los Stored Procedure y el llamado a estos desde los Form.
2. La base de datos cuenta con unos Triggers para la actualizacion de los datos modificados.
3. Descripcion de los form:
   
   1. FCostos:
      Este form se encarga de generar los costos por cada parte del mueble a costear, los requerimientos y operaciones que usa han sido especificados por el cliente.
   
     1.1. FRegistroTipoCosto:
     Este es un form secundario que se encarga de guardar los tipos de costo que seran usados al costear
     1.2. FFormularioCosto:
     eSTE form a pesar de no ser principal es muy importante pues es el encargado de mostrar toda la informacion del costeo de un producto y calcular el porcentage de ganancia , con el cual retorna el rpecio de fabrica, la utilidad y el valor final de este para luego generar un registro en pdf de la operacion.

   2. FInicio:
      Este form es el inicial y tiene los botones de acceso a : FManoObra, FRegistroMateriaPrima, FRegistroProducto y FCostos.

   4. FManoObra :
      Aca se registran los costos y la cantidad de desperdicio por cada tipo de mano de obra\

      4.1. FRegistroTipoManoObra :
      Form secundario encargar de registrar los tipos de mano de obra

   5. FRegistroateriaPria :
      Este for, registra las compras de la materia prima, sus proveedores y otros datos necesarios para la empresa.

      5.2FRegistroUnidadedida:
      Este form secundario registra los tipos de unidades de medida utilizados por las materias primas ademas de definir unos parametros numericos.

   6. FRegistro de Productos : Form para insertar el producto, su referencia y descripcion.
  
## Usado por

Este proyecto fue desarrollado para Muebles NovoArte S.A.S.

