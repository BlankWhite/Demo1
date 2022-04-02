using ConsoleApp7;
using SqlSugar;

SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
{
    ConnectionString = "",
    DbType = DbType.Oracle,
    IsAutoCloseConnection = true
});

var list = db.SqlQueryable<Delivery>(@"SELECT
    t1.po_no,
    t2.do_no,
    t1.cust_id
    || ' '
    || t1.cust_name,
    t3.schedule_date,
    t5.key_part_no,
    replace(upper(t4.box_no), ' ', '') product_sn,
    t5.ean_code,
    t5.stock_code,
    t1.department
FROM
    asshipping.shipping_detail t1,
    delivery.do_line           t2,
    delivery.truck_do         t3,
    delivery.delivery_log      t4,
    sfism4.u_mo_data_t@NBSFIS.NB.GIGABYTE.INTRA                t5
WHERE
        substr(t1.po_no, 0, 10) = t2.so_no
    AND to_number(substr(t1.po_no, 12)) = t2.so_seq
    AND t3.do_no = t2.do_no
    AND t4.sys_parent_id = t2.sys_id
    AND t5.key_part_no = replace(upper(t2.model), ' ', '')
    AND substr(t1.model, 0, 2) IN ( '9R', '9W' )
    AND t1.sys_online = 'Y'
    AND t1.po_no = 'MO3-M10013-001'
GROUP BY
    t1.po_no,
    t2.do_no,
    t1.cust_id,
    t1.cust_name,
    t3.schedule_date,
    t5.key_part_no,
    t4.box_no,
    t5.ean_code,
    t5.stock_code,
    t1.department
ORDER BY
    t1.po_no,
    t4.box_no").ToPageList(1,20);
var t =0;