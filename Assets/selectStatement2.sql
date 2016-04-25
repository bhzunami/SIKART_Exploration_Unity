USE ivis_fs16;
SELECT  w.HAUPTNR, w.JAHR, w.URHAUPTNR, p.SPRCODE, p.GESCHLECHT, p.AUSSTELLUNGEN, p.EVENTS, p.PERFORMANCES, p.LITERATUR, p.RATING, o.PLZ
FROM Werk w
INNER JOIN Person p ON w.URHAUPTNR = p.HAUPTNR 
INNER JOIN Ort o ON LOCATE(o.GEONAME, p.LEBENSDATEN) > 0 
WHERE o.GEOTYPUS = "Ort" 
AND NOT o.GEONAME = "" 
AND NOT o.PLZ="";