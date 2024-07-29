<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {pagingModel} from "@/models/pagingModel";
import {unitRoleModel} from "@/models/unitRoleModel";
import {lienHeModel} from "@/models/lienheModel";

export default {
  page: {
    title: "Quản lý thông tin liên hệ",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader},
  data() {
    return {
      title: "Quản lý thông tin liên hệ ",
      items: [
        {
          text: "Liên hệ",
          href: "/thong-tin-lien-he",
          // active: true,
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      showModal: false,
      showDetail: false,
      showDeleteModal: false,
      submitted: false,
      model: lienHeModel.baseJson(),
      listRole: [],
      pagination: pagingModel.baseJson(),
      totalRows: 1,
      todoTotalRows: 1,
      currentPage: 1,
      numberOfElement: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      filter: null,
      todoFilter: null,
      filterOn: [],
      todofilterOn: [],
      sortBy: "age",
      sortDesc: false,
      fields: [
        {
          key: 'STT', label: 'STT',
          class: "text-center",
          thStyle: {width: '80px', minWidth: '60px'},
          tdClass: 'align-middle',
          thClass: 'hidden-sortable'
        },
        {
          key: "name",
          label: "Tên",
          class: "text-center",
          thStyle: {width: '150px', minWidth: '150px'},
          thClass: 'hidden-sortable'
        },
        {
          key: "diaChi",
          label: "Địa chỉ",
          sortable: true,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        // {
        //   key: "sdt",
        //   label: "Số điện thoại",
        //   class: "text-center",
        //   thStyle: {width: '150px', minWidth: '150px'},
        //   thClass: 'hidden-sortable'
        // },
        // {
        //   key: "fax",
        //   label: "Số fax",
        //   class: "text-center",
        //   thStyle: {width: '150px', minWidth: '150px'},
        //   thClass: 'hidden-sortable'
        // },
        {
          key: "email",
          label: "Email",
          class: "text-center",
          thStyle: {width: '150px', minWidth: '150px'},
          thClass: 'hidden-sortable'
        },
        {
          key: "vanPhongDaiDien",
          label: "Văn phòng đại diện",
          class: "text-center",
          thStyle: {width: '150px', minWidth: '150px'},
          thClass: 'hidden-sortable'
        },
        // {
        //   key: "maSoThue",
        //   label: "Mã số thuế",
        //   class: "text-center",
        //   thStyle: {width: '150px', minWidth: '150px'},
        //   thClass: 'hidden-sortable'
        // },
        {
          key: "nguoiBienTap",
          label: "Người biên tập",
          class: "text-center",
          thStyle: {width: '150px', minWidth: '150px'},
          thClass: 'hidden-sortable'
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: "text-center",
          thStyle: {width: '150px', minWidth: '150px'},
          thClass: 'hidden-sortable'
        }
      ],
    };
  },
  validations: {
    model: {
      name: {required},
      diaChi: {required},
      soDienThoai:{required},
      fax:{required},
      email:{required},
      vanPhongDaiDien:{required},
      maSoThue:{required},
      nguoiBienTap:{required}
    },
  },
  created() {
    this.getThongTin();
    const user = {
      id: 1,
      name: 'Victory Osayi',
      is_editor: true,
      is_admin: false,
      // you can have role based permission list or access control list possibly from database
      permissions: [
        'manage-THONGTINLIENHE-64aab25fba783523f381c447', 
        'create-THONGTINLIENHE-64aab25fba783523f381c447',
        'update-THONGTINLIENHE-64aab25fba783523f381c447',
        'delete-THONGTINLIENHE-64aab25fba783523f381c447'
      ]
    }
  },
  watch: {
    model: {
      deep: true,
      handler(val) {
        // addCoQuanToModel()
        // this.saveValueToLocalStorage()
      }
    },
    showModal(status) {
      if (status == false) this.model = lienHeModel.baseJson();
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },
  methods: {
    async getThongTin(){
      let promise =  this.$store.dispatch("lienheStore/getAll")
      return promise.then(resp => {
        if(resp.data == null){
          return []
        }else{
          if (resp.data != null )
          {
            this.model = resp.data
          }


        }
      })
    },
    async handleSubmitThongTin(e) {
      console.log("LOG THONG TIN ", this.model)
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      console.log("LOG INVALID 36165 : ", this.$v);
      if (this.$v.$invalid) {
        console.log("LOG INVALID  : ")
        return;
      } else {
        console.log("LOG ELSE  INVALID  : ")
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });

        // await this.$store.dispatch("hoaDonStore/getFile").then((res) => {
        //   var blob = new Blob([this.base64ToArrayBuffer(res.data)], {type: "application/pdf"});
        //   var link = document.createElement('a');
        //   link.href = window.URL.createObjectURL(blob);
        //   var fileName = "hoadon";
        //   link.download = fileName;
        //   link.click();
        // })
        await this.$store.dispatch("lienheStore/create", this.model).then((res) => {
          if (res.code === 0) {
            this.model = res.data
            this.showModal = false;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        });
        loader.hide();
        this.submitted = false;

      }
      this.submitted = false;
    },
  }
}
</script>
<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="card">
      <div class="card-body">
        <form ref="formContainer">
          <!-- API Key -->
          <div class="row">
            <div class="col-md-3">
              <label for="">Tên </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    v-model="model.name"
                    type="text"
                    class="form-control"
                    placeholder="Nhập tên"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.name.$error,
                    }"
                >
                <div
                    v-if="submitted && !$v.model.name.required"
                    class="invalid-feedback"
                >
                  Tên không được trống.
                </div>
              </div>
            </div>
          </div>
          <!--Is Ative -->



          <div class="row">
            <div class="col-md-3">
              <label for="">Địa chỉ </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <textarea
                    v-model="model.diaChi"
                    type="text"
                    class="form-control "
                    placeholder="Nhập địa chỉ"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.diaChi.$error,
                    }"
                />
                <div
                    v-if="submitted && !$v.model.diaChi.required"
                    class="invalid-feedback"
                >
                  Địa chỉ không được trống.
                </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Số điện thoại </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    type="text"
                    class="form-control "
                    v-model="model.soDienThoai"
                    placeholder="Nhập số điện thoại"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.soDienThoai.$error,
                    }"
                />
                <div
                    v-if="submitted && !$v.model.soDienThoai.required"
                    class="invalid-feedback"
                >
                  Số điện thoại không được trống.
                </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Số Fax </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    type="text"
                    class="form-control "
                    v-model="model.fax"
                    placeholder="Nhập số fax"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.fax.$error,
                    }"
                />
                <div
                    v-if="submitted && !$v.model.fax.required"
                    class="invalid-feedback"
                >
                  Số fax không được trống.
                </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Email </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    type="text"
                    class="form-control "
                    v-model="model.email"
                    placeholder="Nhập email"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.email.$error,
                    }"
                />
                <div
                    v-if="submitted && !$v.model.email.required"
                    class="invalid-feedback"
                >
                  Email không được trống.
                </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Văn phòng đại diện </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    type="text"
                    class="form-control "
                    v-model="model.vanPhongDaiDien"
                    placeholder="Nhập văn phòng đại diện"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.vanPhongDaiDien.$error,
                    }"
                />
                <div
                    v-if="submitted && !$v.model.vanPhongDaiDien.required"
                    class="invalid-feedback"
                >
                  Văn phòng đại diện không được trống.
                </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Mã số thuế </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    type="number"
                    class="form-control "
                    v-model="model.maSoThue"
                    placeholder="Nhập mã số thuế"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.maSoThue.$error,
                    }"
                />
                <div
                    v-if="submitted && !$v.model.maSoThue.required"
                    class="invalid-feedback"
                >
                  Mã số thuế không được trống.
                </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Người biên tập </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    type="text"
                    class="form-control "
                    v-model="model.nguoiBienTap"
                    placeholder="Nhập người biên tập"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.nguoiBienTap.$error,
                    }"
                />
                <div
                    v-if="submitted && !$v.model.nguoiBienTap.required"
                    class="invalid-feedback"
                >
                  Người biên tập không được trống.
                </div>
              </div>
            </div>
          </div>

          <div class="row">
            <div class="col-md-12 text-center" >
              <b-button
                  v-if="$can('manage-THONGTINLIENHE-64aab25fba783523f381c447') || $can('create-THONGTINLIENHE-64aab25fba783523f381c447')"
                  class="btn cs-btn-primary btn-rounded mb-2 me-2"
                  variant="success"
                  v-on:click="handleSubmitThongTin"
              >
                <i class="bx bx-save "></i>
                Lưu thông tin
              </b-button>
            </div>
          </div>
        </form>
      </div>
    </div>
  </Layout>
</template>
