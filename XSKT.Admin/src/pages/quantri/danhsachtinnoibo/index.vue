<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import {numeric, required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {pagingModel} from "@/models/pagingModel";
import Multiselect from "vue-multiselect";
import {contentModel} from "@/models/contentModel";
import DatePicker from "vue2-datepicker";
import Treeselect from "@riophae/vue-treeselect";
export default {
  page: {
    title: "Danh sách tin nội bộ",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader , DatePicker, Treeselect},
  data() {
    return {
      title: "Bài viết",
      items: [
        {
          text: "Bài viết",
          href: '/tin-noi-bo'
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      fields: [
        { key: 'STT',
          label: 'STT',
          class: 'td-stt',
          sortable: false,
          thClass: 'hidden-sortable'
        },
        {
          key: "soKiHieu",
          label: "Số ký hiệu",
          class: 'td-skh',
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "name",
          label: "Tiêu đề",
          class: 'td-username',
          sortable: false,
          thStyle: "text-align:center",
          thClass: 'hidden-sortable'
        },
        {
          key: "files",
          label: "Tải văn bản",
          class: 'td-download',
          thClass: 'hidden-sortable',
          thStyle: {width: '100px', minWidth: '60px'},
        },
        {
          key: 'process',
          label: 'Xử lý',
          class: 'text-center',
          sortable: false,
          thStyle: {width: '130px', minWidth: '60px'},
          thClass: 'hidden-sortable'
        }
      ],
      currentPage: 1,
      perPage: 10,
      pageOptions: [5, 10, 25, 50, 100],
      showModal: false,
      showDeleteModal: false,
      submitted: false,
      sortBy: "age",
      filter: null,
      sortDesc: false,
      filterOn: [],
      numberOfElement: 1,
      totalRows: 1,
      model: contentModel.baseJson(),
      listCoQuan: [],
      listRole: [],
      pagination: pagingModel.baseJson(),
      itemFilter: {
        content : null,
        ngayBatDau: null,
        ngayKetThuc: null,
        menuId : null
      },
      treeView: [],
      idMenu:null,
      url: `${process.env.VUE_APP_API_URL}filesminio/view`,
    };
  },
  computed: {
    //Validations
    rules(){
      return{
        userName: {required},
        fullName: {required},
        unitRole: {required},
      }
    }
  },
  validations: {
    model: {
      userName: {required},
      fullName: {required},
      unitRole: {required},
    },
  },
  created() {
    const user = {
      id: 1,
      name: 'Victory Osayi',
      is_editor: true,
      is_admin: false,
      // you can have role based permission list or access control list possibly from database
      permissions: [
        'manage-QUANLYTINNOIBO-656c6e714300fbc7af15838a', 
        'create-QUANLYTINNOIBO-656c6e714300fbc7af15838a',
        'update-QUANLYTINNOIBO-656c6e714300fbc7af15838a',
        'delete-QUANLYTINNOIBO-656c6e714300fbc7af15838a',
        'detail-QUANLYTINNOIBO'
      ]
    }
    this.GetTreeList();
  },
  methods: {
    normalizer(node){
      if(node.children == null || node.children == 'null'){
        delete node.children;
      }
    },
    async GetTreeList(){
      await this.$store.dispatch("menuStore/getTreeListNoiBo").then((res) => {
        this.treeView = res.data;
        //  console.log("LOG TREE VIEW : " , this.treeView)
      })
    },
    async fnGetList() {
         this.$refs.tblList?.refresh()
    },
    async getListRole(){
      await  this.$store.dispatch("unitRoleStore/getAll").then((res) =>{
        this.listRole = res.data || [];
      })
    },
    async handleDelete() {
      if (this.model._id != 0 && this.model._id != null && this.model._id) {
        await this.$store.dispatch("contentNoiBoStore/delete",{_id :  this.model._id}).then((res) => {
          if (res.code===0) {
            this.fnGetList();
            this.showDeleteModal = false;
          }
          var a = {
            message: res.message,
            code: res.code
          };
          // console.log("LOG A : " ,a)
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code
          });
        });
      }
    },
    handleShowDeleteModal(id) {
      this.model._id = id;
      this.showDeleteModal = true;
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.model._id != 0 &&
            this.model._id != null &&
            this.model._id
        ) {
     //     console.log("LOG USER UPDATE NE : " , this.model)
          // Update model
          await this.$store.dispatch("contentNoiBoStore/update", this.model).then((res) => {
            if (res.code === 0) {
              this.showModal = false;
              this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
        } else {
          // Create model
       //   console.log("LOG USER CREATE NE : " , this.model)
          await this.$store.dispatch("contentNoiBoStore/create", this.model).then((res) => {
            if (res.code === 0) {
              this.fnGetList();
              this.showModal = false;
              this.model={}
            }
            this.$store.dispatch("snackBarStore/addNotify", {
              message: res.message,
              code: res.code,
            });
          });
        }
        loader.hide();
      }
      this.submitted = false;
    },
    async handleDetail(id) {
      await this.$store.dispatch("contentNoiBoStore/getById", {_id : id}).then((res) => {
        if (res.code === 0) {
          console.log("123:",res);
          this.model = contentModel.fromJson(res.data);
         // console.log("LOG MODEL DETAIL ", this.model)
          this.showModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code,
          });
        }
      });
    },
    async handleUpdate(id) {
      await this.$store.dispatch("contentNoiBoStore/getByMenuId", {_id : id}).then((res) => {
        if (res.code===0) {
          console.log(res)
          this.model = contentModel.toJson(res.data);
          this.showModal = true;
        } else {
          this.$store.dispatch("snackBarStore/addNotify", {
            message: res.message,
            code: res.code,
          });
        }
      });
    },
    addCoQuanToModel(node, instanceId ){
      if(node.id){
        this.itemFilter.menuId = node.id;
      }
    },
    myProvider(ctx) {
      // Lấy menuId từ Local Storage
      const storedMenuId = localStorage.getItem("currentMenuId");

      const params = {
        start: ctx.currentPage,
        limit: ctx.perPage,
        content: this.itemFilter.content,
        menuId: this.$route.params.id,
        startDate: this.itemFilter.ngayBatDau
          ? new Date(this.itemFilter.ngayBatDau)
          : null,
        endDate: this.itemFilter.ngayKetThuc
          ? new Date(this.itemFilter.ngayKetThuc)
          : null,
        sortDesc: ctx.sortDesc,
      };

      this.loading = true;
      try {
        let promise = this.$store.dispatch("contentNoiBoStore/getPagingParams", params);
        return promise.then((resp) => {
          let items = resp.data.data;
          this.totalRows = resp.data.totalRows;
          this.numberOfElement = resp.data.data.length;
          this.loading = false;
          return items || [];
        });
      } finally {
        this.loading = false;
      }
    },

    handleSearch() {
      // console.log("LOG HANDLE SEARCH LICH SU GIAO DICH " , this.itemFilter)
      this.$refs.tblList.refresh();
    },
    handleClear(){
      this.itemFilter = {
        content : null,
        ngayBatDau: null,
        ngayKetThuc: null,
        menuId : null
      } ;
      this.$refs.tblList.refresh();
    },
    getColorWithExtFile(ext) {
      if (ext == '.docx' || ext == '.doc')
        return 'text-primary';
      else if (ext == '.xlsx' || ext == '.xls')
        return 'text-success';
      else if (ext == '.pdf')
        return 'text-danger';
      else return 'text-danger';
    },

    getIconWithExtFile(ext) {
      if (ext == '.docx' || ext == '.doc')
        return 'mdi mdi-microsoft-word';
      else if (ext == '.xlsx' || ext == '.xls')
        return 'mdi mdi-microsoft-excel';
      else if (ext == '.pdf')
        return 'mdi mdi-file-pdf-outline';
      else if (ext == '.pptx')
        return 'fas fa-file-powerpoint';
      else return 'mdi-file-clock-outline';
    },
  },
  mounted() {
    this.getListRole();
  },
  watch: {
    model: {
      deep: true,
      handler(val) {
      }
    },
    showModal(status) {
      if (status == false) this.model = contentModel.baseJson();
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
    $route(to, from) {
       this.$refs.tblList.refresh();
    },
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row">
              <div class="mb-3 col-lg-3">
                <label>Nội dung</label>
                <input
                    v-model="itemFilter.content"
                    type="text"
                    name="untyped-input"
                    class="form-control"
                    placeholder="Tìm kiếm theo tiêu đề"
                    style="height: 39px;"
                />
              </div>
              <div class="mb-3 col-lg-3">
                <label>Thời gian bắt đầu</label>
                <date-picker
                    v-model="itemFilter.ngayBatDau"
                    format="DD/MM/YYYY"
                ></date-picker>
              </div>
              <div class="mb-3 col-lg-3">
                <label>Thời gian kết thúc</label>
                <date-picker
                    v-model="itemFilter.ngayKetThuc"
                    format="DD/MM/YYYY"
                ></date-picker>
              </div>
              <div class="mb-3 col-lg-3">
                <label>Chuyên mục tin</label>
                <treeselect
                    v-on:select="addCoQuanToModel"
                    :normalizer="normalizer"
                    :options="treeView"
                    :value="itemFilter.menuId"
                    :multiple="false"

                    placeholder="Chọn chuyên mục tin"
                >
                  <label style="height: 39px;" slot="option-label" slot-scope="{ node, shouldShowCount, count, labelClassName, countClassName }" :class="labelClassName">
                    {{ node.label }}
                    <span v-if="shouldShowCount" :class="countClassName">({{ count }})</span>
                  </label>
                </treeselect>
              </div>
            </div>
            <div class="row">
              <div class="col-12 text-center">
                <b-button variant=""
                          class="w-10 btn-search"
                          style="margin-right: 10px ; height: 40px ; width: 130px; font-size: 14px; background-color: #F2B705; border: none; color: #000 !important; font-weight: bold;"
                          size="sm"
                          @click="handleSearch"
                >
                  <i
                      class="bx bx-search font-size-16 align-middle me-2"
                  ></i>
                  Tìm kiếm
                </b-button>
                <b-button
                    class="w-10 btn-reset"
                    style="margin-right: 10px; height: 40px ; width: 130px; font-size: 14px; background-color: #F2B705; border: none; color: #000 !important; font-weight: bold;"
                    size="sm"
                    @click="handleClear"
                >
                  <i class="mdi mdi-replay font-size-16 align-middle me-2"></i>
                  Làm mới
                </b-button>
              </div>
            </div>
            <b-modal
                v-model="showModal"
                title="Thông tin chi tiết bài viết nội bộ"
                title-class="text-black font-18"
                body-class="p-3"
                hide-footer
                centered
                scrollable
                no-close-on-backdrop
                size="xl"
            >
              <form @submit.prevent="handleSubmit"
                    ref="formContainer">
                    <div class="row">
                      <div class="col-lg-12 mb-3">
                        <div class="title mt-4 mb-4">
                          <h2>
                              {{ this.model.name }}
                          </h2>
                        </div>
                        <div v-html="model.noiDung" class="noidung" style="font-size: 14px;">
                        </div>
                        <div v-for ="(item,index) in this.model.files" :key="index" style="margin-top: 20px; border-top: 1px solid #ccc;">
                            <a
                                :href="`${url}/${item.fileId}`"
                                class="text-dark fw-medium d-flex justify-content-between align-items-center">
                                <div class="font-size-13">
                                    <span  style="font-weight: bold; color: #990000;">TỆP ĐÍNH KÈM
                                    <i class="mdi mdi-chevron-double-right font-size-20" aria-hidden="true"></i>
                                  </span>
                                <i
                                    :class="`${getColorWithExtFile(item.ext)} me-2 ${getIconWithExtFile(item.ext)}`"
                                ></i>
                                <span style="font-weight: bold;color: rgba(var(--bs-link-color-rgb), var(--bs-link-opacity, 1));">{{ item.fileName }}</span>
                                </div>
                            </a>
                        </div>
                      </div>
                    </div>
                <div class="text-end pt-2 mt-3">
                  <b-button variant="light" @click="showModal = false">
                    Đóng
                  </b-button>
                </div>
              </form>
            </b-modal>
            <div class="row">
              <div class="col-12">
                <div class="row mt-4 mb-2">
                  <div class="col-sm-12 col-md-4">
                    <div
                        class="col-sm-12 d-flex justify-content-left align-items-center"
                    >
                      <div
                          id="tickets-table_length"
                          class="dataTables_length m-1"
                          style="
                        display: flex;
                        justify-content: left;
                        align-items: center;
                      "
                      >
                        <div class="me-1" >Hiển thị </div>
                        <b-form-select
                            class="form-select form-select-sm"
                            v-model="perPage"
                            size="sm"
                            :options="pageOptions"
                            style="width: 70px"
                        ></b-form-select
                        >&nbsp;
                        <div style="width: 100px"> dòng </div>
                      </div>
                    </div>
                  </div>
                  <!-- <div class="col-sm-12 col-md-8">
                    <input
                        v-model="itemFilter.content"
                        type="text"
                        name="untyped-input"
                        class="form-control mt-1"
                        placeholder="Tìm kiếm theo tiêu đề"
                    />
                  </div> -->
                </div>
                <div class="table-responsive mb-0">
                  <b-table
                      class="datatables table-admin"
                      :items="myProvider"
                      :fields="fields"
                      striped
                      bordered
                      responsive="sm"
                      :per-page="perPage"
                      :current-page="currentPage"
                      :sort-by.sync="sortBy"
                      :sort-desc.sync="sortDesc"
                      :filter="filter"
                      :filter-included-fields="filterOn"
                      ref="tblList"
                      primary-key="id"
                  >
                    <template v-slot:cell(STT)="data">
                      {{ data.index + ((currentPage-1)*perPage) + 1  }}
                    </template>
                    <!-- <template v-slot:cell(menu)="data">
                     <span style="margin-left: 5px">
                       {{data.item.menu.name}}
                     </span>
                    </template> -->
                    <template v-slot:cell(soKiHieu)="data">
                      <span>{{ data.item.soKiHieu }}</span>
                      <div class="row ngayKi">
                        <span>Ngày ký: {{ data.item.ngayKyShow }}</span>
                      </div>
                    </template>
                    <template v-slot:cell(menu)="data">
                      <div v-for ="(item,index) in data.item.menu" :key="index">
                        <span class="badge bg-warning mb-1" style="padding: 5px; color: #1e2125; text-transform: uppercase; font-weight: bold;">{{ item.name }}</span>
                      </div>
                      </template>
                    <template v-slot:cell(isPublic)="data">
                      <span v-if="data.item.isPublic" class="badge bg-success" style="padding: 5px;">Xuất bản</span>
                      <span v-else class="badge" style="padding: 5px; background-color: red;">Không xuất bản</span>
                    </template>
                    <template v-slot:cell(files)="data">

                      <div v-for ="(item,index) in data.item.files" :key="index">
                        <a
                            :href="`${url}/${item.fileId}`"
                            class="text-dark fw-medium d-flex justify-content-center align-items-center">
                          <div class="">
                            <!-- <i
                                :class="`${getColorWithExtFile(item.ext)} me-2 ${getIconWithExtFile(item.ext)}`"
                            ></i>
                            <span style="font-weight: bold">{{ item.fileName }}</span> -->
                            <i class="mdi mdi-cloud-download" style="font-size: 24px; color: #B50027;"></i>
                          </div>
                        </a>
                      </div>
                      </template>
                    <template v-slot:cell(process)="data">
                      <button
                          v-if="$can('manage-QUANLYTINNOIBO-656c6e714300fbc7af15838a') || $can('detail-QUANLYTINNOIBO')"
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          data-toggle="tooltip" data-placement="bottom" title="Chi tiết"
                          v-on:click="handleDetail(data.item._id)">
                        <i class="mdi mdi-eye me-1 fs-15"></i>
                      </button>
                      <!-- <router-link
                          :to="{
                                      path: `/danh-sach/chi-tiet-noi-bo/${data.item._id}`,
                                    }">
                        <button
                            v-if="$can('manage-QUANLYTINNOIBO-656c6e714300fbc7af15838a') || $can('update-QUANLYTINNOIBO-656c6e714300fbc7af15838a')"
                            type="button"
                            size="sm"
                            class="btn btn-outline btn-sm"
                            >
                          <i class="fas fa-pencil-alt text-success me-1"></i>
                        </button>
                      </router-link> -->
                      <button
                          v-if="$can('manage-QUANLYTINNOIBO-656c6e714300fbc7af15838a') || $can('delete-QUANLYTINNOIBO-656c6e714300fbc7af15838a')"
                          type="button"
                          size="sm"
                          class="btn btn-outline btn-sm"
                          v-on:click="handleShowDeleteModal(data.item._id)">
                        <i class="fas fa-trash-alt text-danger me-1"></i>
                      </button>
                    </template>
                  </b-table>
                </div>
                <div class="row">
                  <b-col>
                    <div>Hiển thị {{numberOfElement}} trên tổng số {{totalRows}} dòng</div>
                  </b-col>
                  <div class="col">
                    <div
                        class="dataTables_paginate paging_simple_numbers float-end">
                      <ul class="pagination pagination-rounded mb-0">
                        <!-- pagination -->
                        <b-pagination
                            v-model="currentPage"
                            :total-rows="totalRows"
                            :per-page="perPage"
                        ></b-pagination>
                      </ul>
                    </div>
                  </div>
                </div>

              </div>
            </div>
            <b-modal
                v-model="showDeleteModal"
                centered
                title="Xóa dữ liệu"
                title-class="font-18"
                no-close-on-backdrop
            >
              <p>
                Dữ liệu xóa sẽ không được phục hồi!
              </p>
              <template #modal-footer>
                <button v-b-modal.modal-close_visit
                        class="btn btn-outline-info m-1"
                        v-on:click="showDeleteModal = false">
                  Đóng
                </button>
                <button v-b-modal.modal-close_visit
                        class="btn btn-danger btn m-1"
                        v-on:click="handleDelete">
                  Xóa
                </button>
              </template>
            </b-modal>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
<style>
.td-stt {
  text-align: center;
}
.td-xuly {
  text-align: center;
  width: 10% !important;
}

.td-chuyenmuc{
  width: 20%;
}
.td-trangthai{
  width: 10%;
}
.vue-treeselect__control{
  height: 39px !important;
}
.mx-table .mx-table-date .table thead th, thead, th {
  background-color: rgb(255, 255, 255)!important;
  color: #0e0e0e !important;
  border: none;
}
.mx-calendar-content .cell.active {
  color: #fff;
  background-color: #1284e7 !important;
}
.modal-dialog{
  max-width: 90%;
}

.td-skh{
  width: 20%;
}
</style>
