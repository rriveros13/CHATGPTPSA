select * from ge_cta_clientes
where relacion='C';

select pe.nro_documento, 
        pr.nro_cuenta, 
        pr.can_cuotas, 
        pr.mto_capital, 
        pr.sal_capital, 
        pr.sal_interes, 
        pr.fec_inicio, 
        pr.fec_vencimiento, 
        pr.en_juicio
from pr_cta_prestamos pr
inner join ge_cta_clientes ct on pr.nro_cuenta = ct.nro_cuenta and ct.relacion = 'P'
inner join ba_personas pe on ct.cod_persona = pe.cod_persona
where pr.nro_cuenta=66001246;

--select * from pr_cta_prestamos where estado='M';

/*
V - prestamo vencido
M - prestamo moroso
C - prestamo vigente, activo
N - cancelado, pagado en su totalidad
G - prestamo en gestion (cobradores gestionan dichos prestamos)
I - prestamo incobrable
*/

select count(*) from pr_cta_prestamos where sal_capital+sal_interes=0 and en_juicio='N';
--where sal_capital=0 and en_juicio='S';


select count(*) from (
select documento, prestamonumero
from pv_prestamos
group by documento, prestamonumero);

select count(*) from pv_prestamos; --18596

select distinct en_juicio from pr_cta_prestamos;

select count(*) from pr_cuotas where nro_cuota > 0; --18596
select * from pr_cuotas; 

select count(*) from pr_cta_prestamos;