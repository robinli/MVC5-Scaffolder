//筛选枚举
//var TrueOrFalse = [{ value: '', text: 'All' }, { value: 1, text: '是' }, { value: 0, text: '否' }];
var Unit = [{ value: '', text: 'All' }, { value: 'EA', text: 'EA' }, { value: 'KG', text: 'KG' }, { value: 'PCS', text: 'PCS' }, { value: 'SET', text: 'SET' }];
var Currency = [{ value: '', text: 'All' }, { value: 'CNY', text: '人民币' }, { value: 'USD', text: '美元' }, { value: 'GBP', text: '英镑' }, { value: 'EUR', text: '欧元' }];
var CreditRating = [{ value: '', text: 'All' }, { value: '0', text: '优' }, { value: '1', text: '良' }, { value: '2', text: '中' }, { value: '3', text: '差' }];
var Package = [{ value: '', text: 'All' }, { value: '120', text: '箱' }, { value: '121', text: '批' }, { value: '125', text: '包' }, { value: '140', text: '盒' }];
var MakeFlag = [{ value: '', text: 'All' }, { value: '0', text: '采购件' }, { value: '1', text: '生产件' }];
var Category = [{ value: '', text: 'All' }, { value: '0', text: '成品' }, { value: '1', text: '半成品' }, { value: '2', text: '原材料' }];
var Area = [{ value: '', text: 'All' }, { value: '一库', text: '一库' }, { value: '二库', text: '二库' }, { value: '三库', text: '三库' }];
var PreferredVendorStatus = [{ value: '', text: 'All' }, { value: '1', text: '是' }, { value: '0', text: '否' }];
var ActiveFlag = [{ value: '', text: 'All' }, { value: '1', text: '是' }, { value: '0', text: '否' }];
var ShipMethod = [{ value: '', text: 'All' }, { value: '0', text: '陆运' }, { value: '1', text: '海运' }, { value: '2', text: '空运' }];

var DeliverStatus = [{ value: '0', text: '未发货' }, { value: '1', text: '运货中' }, { value: '2', text: '已到货' }];
var Status = [{ value: '', text: 'All' }, { value: '0', text: '未发布' }, { value: '1', text: '已发布' }, { value: '2', text: '承诺中' }, { value: '3', text: '审核不通过' }, { value: '4', text: '审核通过' }, { value: '5', text: '发货中' }, { value: '6', text: '已收货' }, { value: '7', text: '部分已承诺交期' }, { value: '8', text: '全部已承诺交期' }, { value: '9', text: '部分已收货' }, { value: '10', text: '全部已收货' }];
var PromiseStatus = [{ value: '', text: 'All' }, { value: '1', text: '已发布' }, { value: '2', text: '承诺中' }, { value: '3', text: '审核不通过' }, { value: '4', text: '审核通过' }, { value: '5', text: '发货中' }, { value: '6', text: '已收货' }];
var ConfirmStatus = [{ value: '', text: 'All' }, { value: '2', text: '承诺中' }, { value: '3', text: '审核不通过' }, { value: '4', text: '审核通过' }, { value: '5', text: '发货中' }, { value: '6', text: '已收货' }];
var TraceStatus = [{ value: '', text: 'All' },{ value: '4', text: '审核通过' }, { value: '5', text: '发货中' }, { value: '6', text: '已收货' }];
var ReceivingStatus = [{ value: '', text: 'All' }, { value: '5', text: '发货中' }, { value: '6', text: '已收货' }];

var MessageGroup = [{ value: '', text: 'All' }, { value: '0', text: '系统操作' }, { value: '1', text: '业务操作' }, { value: '2', text: '接口操作' }];
var MessageType = [{ value: '', text: 'All' }, { value: '0', text: 'Information' }, { value: '1', text: 'Error' }, { value: '2', text: 'Alert' }];
var MessageNew = [{ value: '', text: 'All' }, { value: '0', text: '未读' }, { value: '1', text: '已读' }];
var MessageNotice = [{ value: '', text: 'All' }, { value: '0', text: '未发' }, { value: '1', text: '已发' }];
var ReceivingStatus = [{ value: '', text: 'All' }, { value: '5', text: '发货中' }, { value: '6', text: '已收货' }];
var SplitStatus = [{ value: '0', text: '暂存' }, { value: '1', text: '待审批' }, { value: '2', text: '审批不通过' }, { value: '3', text: '审批通过' }];

var VerType = [{ value: 0, text: '原始版' }, { value: 1, text: '修正版' }];
//编辑选择
//var GCTrueOrFalse = [{ value: 1, text: '是' }, { value: 0, text: '否' }];
var GCPreferredVendorStatus = [{ value: 1, text: '是' }, { value: 0, text: '否' }];
var GCVerType = [{ value: 0, text: '原始版' }, { value: 1, text: '修正版' }];
var GCActiveFlag = [{ value: 1, text: '是' }, { value: 0, text: '否' }];
var GCUnit = [{ value: 'EA', text: 'EA' }, { value: 'KG', text: 'KG' }, { value: 'PCS', text: 'PCS' }, { value: 'SET', text: 'SET' }];
var GCCurrency = [ { value: 'CNY', text: '人民币' }, { value: 'USD', text: '美元' }, { value: 'GBP', text: '英镑' }, { value: 'EUR', text: '欧元' }];
var GCCreditRating = [ { value: 0, text: '优' }, { value: 1, text: '良' }, { value: 2, text: '中' }, { value: 3, text: '差' }];
var GCPackage = [{ value: '120', text: '箱' }, { value: '121', text: '批' }, { value: '125', text: '包' }, { value: '140', text: '盒' }];
var GCMakeFlag = [{ value: '0', text: '采购件' }, { value: '1', text: '生产件' }];
var GCCategory = [{ value: '0', text: '成品' }, { value: '1', text: '半成品' }, { value: '2', text: '原材料' }];
var GCArea = [{ value: '一库', text: '一库' }, { value: '二库', text: '二库' }, { value: '三库', text: '三库' }];
var GCShipMethod = [{ value: '0', text: '陆运' }, { value: '1', text: '海运' }, { value: '2', text: '空运' }];
var GCStatus = [{ value: '0', text: '未发布' }, { value: '1', text: '已发布' }, { value: '2', text: '承诺中' }, { value: '3', text: '审核不通过' }, { value: '4', text: '审核通过' }, { value: '5', text: '发货中' }, { value: '6', text: '已收货' }, { value: '7', text: '部分已承诺交期' }, { value: '8', text: '全部已承诺交期' }, { value: '9', text: '部分已收货' }, { value: '10', text: '全部已收货' }];
var GCSplitStatus = [{ value: '', text: 'ALL' }, { value: '暂存', text: '暂存' }, { value: '待审批', text: '待审批' }, { value: '审批不通过', text: '审批不通过' }, { value: '审批通过', text: '审批通过' }];

 