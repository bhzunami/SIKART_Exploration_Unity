USE ivis_fs16;
SELECT * FROM ivis_fs16.Werk w
INNER JOIN  Person p ON w.Urhauptnr = p.hauptnr;

SELECT w.Objektbez, w.Jahr, p.namident, p.sprcode, p.Geschlecht, p.gebjahr, p.todjahr, p.ausstellungen, p.events, p.performances, p.literatur, p.rating, p.NATIONALITAET FROM ivis_fs16.Werk w
INNER JOIN  Person p ON w.Urhauptnr = p.hauptnr;