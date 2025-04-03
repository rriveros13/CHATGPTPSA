select * from pv_prestamos order by fechatransaccion desc;

select count(*) from pv_prestamos; --18655

-- cantidad total de prestamos
select count(*) from (
select documento, prestamonumero
from pv_prestamos
group by documento, prestamonumero); --2351

-- cantidad de prestamos activos
select count(*) from (
select documento, prestamonumero, activo
from pv_prestamos
group by documento, prestamonumero, activo
having activo=1);

-- cantidad de prestamos cancelados
select count(*) from (
select documento, prestamonumero, activo
from pv_prestamos
group by documento, prestamonumero, activo
having activo=0 and sum(saldocapital)+sum(saldointeres)=0);

-- cantidad de prestamos activos sin juicio
select count(*) from (
select documento, prestamonumero, activo, enjuicio
from pv_prestamos
group by documento, prestamonumero, activo, enjuicio
having activo=1 and enjuicio='N');

-- cantidad de prestamos en juicio
select count(*) from (
select documento, prestamonumero, activo, enjuicio
from pv_prestamos
group by documento, prestamonumero, activo, enjuicio
having activo=1 and enjuicio='S');

select count(*) from pr_cta_prestamos where sal_capital+sal_interes=0 and en_juicio='N';

select count(*) from (
select documento, prestamonumero, documentovinculo1 
from pv_prestamos
group by documento, prestamonumero, documentovinculo1 --2351
having documentovinculo1 is not null);

------------------------------------------------------

select  d.cod_persona,
        p.nro_documento,
        p.nom_completo,
        d.direccion,
        c.cod_ciudad,
        c.descripcion
from ba_direcciones d
inner join ba_personas p on d.cod_persona=p.cod_persona
inner join ba_ciudades c on d.cod_ciudad=c.cod_ciudad;