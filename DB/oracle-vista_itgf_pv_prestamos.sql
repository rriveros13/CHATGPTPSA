select pe.nro_documento Documento,
    a.nro_documento DocumentoVinculo1,
    b.nro_documento DocumentoVinculo2,
   pr.nro_cuenta PrestamoNumero,
   gt.val_parametro PrestamoNumeroOriginal,
   to_number(substr(descripcion, 1, (instr(descripcion, ' ')-1))) SolicitudNumero,
   pr.can_cuotas CantidadCuotas,
   cu.nro_cuota CuotaNumero,
   pr.mto_capital MontoPrestamo,
   case
    when gt.val_parametro is not null then cu.mto_capital+cu.mto_interes
    else cu.mto_capital+cu.mto_interes+round((cu.mto_interes*0.1), 0)
   end MontoCuota,   
   case
    when gt.val_parametro is not null then cu.sal_capital+cu.sal_interes
    else cu.sal_capital+cu.sal_interes+round((cu.sal_interes*0.1), 0)
   end SaldoCuota,
   pr.sal_capital SaldoCapital,
   pr.sal_interes SaldoInteres,   
   cu.fec_vencimiento FechaVencimiento,
   cu.fec_ult_pago FechaPago,
   case
    when cu.fec_ult_pago is null and to_date(to_char(current_date, 'yyyy-mm-dd'), 'yyyy-mm-dd') - cu.fec_vencimiento > 0 then to_date(to_char(current_date, 'yyyy-mm-dd'), 'yyyy-mm-dd') - cu.fec_vencimiento
    when cu.fec_ult_pago is not null and cu.fec_ult_pago - cu.fec_vencimiento > 0 then cu.fec_ult_pago - cu.fec_vencimiento
    else 0
   end Diferencia,
   case
    when cu.sal_capital > 0 then 1
    when cu.sal_capital = 0 then 0
    else 0
   end Activo,
   pr.fec_inicio FechaTransaccion,
   pr.en_juicio EnJuicio
from pr_cta_prestamos pr
inner join ge_cta_clientes ct on pr.nro_cuenta = ct.nro_cuenta and ct.relacion = 'P'
inner join ba_personas pe on ct.cod_persona = pe.cod_persona
inner join pr_cuotas cu on pr.nro_cuenta = cu.nro_cuenta and cu.nro_cuota > 0
left join ge_cta_dat_adicionales gt on pr.nro_cuenta = gt.nro_cuenta and gt.parametro = 'CRM_NRO_OPERACION'
left join (
    select  row_number() over (partition by c.nro_cuenta order by p.nro_documento) as numeracion, 
        p.nro_documento, 
        c.nro_cuenta, 
        c.relacion
    from ge_cta_clientes c
    inner join ba_personas p on c.cod_persona=p.cod_persona
    group by c.relacion, c.nro_cuenta, p.nro_documento
    having c.relacion='C'
) a on a.nro_cuenta=pr.nro_cuenta and a.numeracion=1
left join (
    select  row_number() over (partition by c.nro_cuenta order by p.nro_documento) as numeracion, 
        p.nro_documento, 
        c.nro_cuenta, 
        c.relacion
    from ge_cta_clientes c
    inner join ba_personas p on c.cod_persona=p.cod_persona
    group by c.relacion, c.nro_cuenta, p.nro_documento
    having c.relacion='C'
) b on b.nro_cuenta=pr.nro_cuenta and b.numeracion=2
where pr.nro_solicitud is not null
order by pr.nro_cuenta, cu.nro_cuota