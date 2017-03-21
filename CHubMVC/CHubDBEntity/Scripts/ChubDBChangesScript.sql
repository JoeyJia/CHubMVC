----DB Chagnes Script
----From 3-21-17

--1.For text chagnes Will Bill to Way Bill
update app_pages  set display='-> Way Bill Data Entry' , description ='--- Way Bill Information Load (Import Part Tracking)'
where page_name='wbentry';