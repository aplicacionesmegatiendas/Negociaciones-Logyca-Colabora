alter table Cotizacion
add ct_porc_dscto2 decimal(5, 2),
	ct_evento_comercial2 varchar(150),
	ct_fecha_ini2	date,
	ct_fecha_fin2	date,
	ct_porc_dscto3	decimal(5, 2),
	ct_evento_comercial3	varchar(150),
	ct_fecha_ini3	date,
	ct_fecha_fin3	date

alter table CambioPrecio
add cp_porc_dscto2	decimal(5, 2),
	cp_evento_comercial2	varchar(150),
	cp_fecha_ini2	date,
	cp_fecha_fin2	date,
	cp_porc_dscto3	decimal(5, 2),
	cp_evento_comercial3	varchar(150),
	cp_fecha_ini3	date,
	cp_fecha_fin3	date