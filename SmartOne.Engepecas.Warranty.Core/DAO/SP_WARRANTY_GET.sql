ALTER PROCEDURE SP_WARRANTY_GET
(
	dateFrom 	date,
	dateTo		date,
	wc			nvarchar(30),
	cardCode	nvarchar(30),
	serial		nvarchar(100)
)
LANGUAGE SQLSCRIPT
AS
BEGIN

	SELECT
		-- Header
		OSCL."internalSN" "Serial",
		OSCL."custmrName" "CardName",
		OSCL."U_ENG_NumWC" "WC",
		OSCL."createDate" "CreateDate",
		OSCL."createTime" "CreateTime",
		-- Grid
		OSCL."callID" "Id",
		SCL6."SrcvCallID" || '/' || SCL6."Line" "Service",
		CASE 
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento KM' THEN TO_DECIMAL(SCL6."U_ENG_KM", 10, 2) || ' KM'
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento HR' THEN TO_DECIMAL(SCL6."ActualDur", 10, 2) || ' HT'
			ELSE SCL6."U_ENG_Tipo" || ' - ' || IFNULL(SCL6."Remark", '')
		END "Description",
		MULT."U_Mult"	"Multiplier",
		CASE 
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento KM' THEN SCL6."U_ENG_KM" * MULT."U_Mult"
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento HR' THEN SCL6."ActualDur" * MULT."U_Mult"
			ELSE SCL6."U_ENG_NFValor"
		END "Total",
		CASE 
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento KM' THEN SCL6."U_ENG_KM" * MULT."U_Mult"
			WHEN SCL6."U_ENG_Tipo" = 'Atendimento HR' THEN SCL6."ActualDur" * MULT."U_Mult"
			ELSE SCL6."U_ENG_NFValor" + (SCL6."U_ENG_NFValor" * MULT."U_Mult" / 100)
		END "Balance",
		-- Control fields
		SCL6."Line",
		SCL6."U_ENG_GAR_DATA" "WarDate",
		SCL6."U_ENG_GAR_LCM" "TransId"
	FROM OSCL
		INNER JOIN SCL6 ON SCL6."SrcvCallID" = OSCL."callID"
		LEFT JOIN "@ENG_SERV_MULT" MULT ON MULT."U_Tipo" = "U_ENG_Tipo" AND IFNULL(OSCL."U_ENG_DataWC", OSCL."createDate") BETWEEN MULT."U_VigenciaDe" AND MULT."U_VigenciaAte"
	WHERE OSCL."createDate" BETWEEN dateFrom AND dateTo
	AND (OSCL."U_ENG_NumWC" = IFNULL(wc, '') OR wc = '')
	AND (OSCL."customer" = IFNULL(cardCode, '') OR IFNULL(cardCode, '') = '')
	AND (OSCL."internalSN" = IFNULL(serial, '') OR IFNULL(serial, '') = '')
	AND SCL6."U_ENG_Pago" = 'N'
	AND OSCL."callType" = 2
	AND (SCL6."U_ENG_RespHR" = 'JCB' OR SCL6."U_ENG_RespKM" = 'JCB' OR SCL6."U_ENG_RespNF" = 'JCB')
	ORDER BY OSCL."callID";
END; 
