﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ICMA.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UrbanRFPEntities : DbContext
    {
        public UrbanRFPEntities()
            : base("name=UrbanRFPEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<address_x_contact> address_x_contact { get; set; }
        public virtual DbSet<address_x_organization> address_x_organization { get; set; }
        public virtual DbSet<co_address> co_address { get; set; }
        public virtual DbSet<co_contact> co_contact { get; set; }
        public virtual DbSet<co_favourite> co_favourite { get; set; }
        public virtual DbSet<co_organization> co_organization { get; set; }
        public virtual DbSet<co_page_view> co_page_view { get; set; }
        public virtual DbSet<co_permission> co_permission { get; set; }
        public virtual DbSet<contact_x_organization> contact_x_organization { get; set; }
        public virtual DbSet<contact_x_product> contact_x_product { get; set; }
        public virtual DbSet<contact_x_RACI> contact_x_RACI { get; set; }
        public virtual DbSet<contact_x_request> contact_x_request { get; set; }
        public virtual DbSet<contact_x_response> contact_x_response { get; set; }
        public virtual DbSet<lkup_cxr_rlt_code> lkup_cxr_rlt_code { get; set; }
        public virtual DbSet<lkup_population> lkup_population { get; set; }
        public virtual DbSet<lkup_RACI> lkup_RACI { get; set; }
        public virtual DbSet<lkup_rfp_type> lkup_rfp_type { get; set; }
        public virtual DbSet<lkup_solo_multiple> lkup_solo_multiple { get; set; }
        public virtual DbSet<product_x_request> product_x_request { get; set; }
        public virtual DbSet<ProductSubtype> ProductSubtypes { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<rfp_attachment> rfp_attachment { get; set; }
        public virtual DbSet<rfp_event> rfp_event { get; set; }
        public virtual DbSet<rfp_event_invitee> rfp_event_invitee { get; set; }
        public virtual DbSet<rfp_page> rfp_page { get; set; }
        public virtual DbSet<rfp_product> rfp_product { get; set; }
        public virtual DbSet<rfp_request> rfp_request { get; set; }
        public virtual DbSet<rfp_respond_invitation> rfp_respond_invitation { get; set; }
        public virtual DbSet<rfp_response> rfp_response { get; set; }
        public virtual DbSet<rfp_scope_of_work> rfp_scope_of_work { get; set; }
        public virtual DbSet<rfp_score> rfp_score { get; set; }
        public virtual DbSet<rfp_score_rules> rfp_score_rules { get; set; }
        public virtual DbSet<rfp_sop_section> rfp_sop_section { get; set; }
        public virtual DbSet<rfp_task> rfp_task { get; set; }
        public virtual DbSet<rfp_timeline_master_template> rfp_timeline_master_template { get; set; }
        public virtual DbSet<rfp_timeline_org_template> rfp_timeline_org_template { get; set; }
        public virtual DbSet<rfp_validation_reference> rfp_validation_reference { get; set; }
        public virtual DbSet<rfp_vendor_allowance> rfp_vendor_allowance { get; set; }
        public virtual DbSet<score_x_request> score_x_request { get; set; }
        public virtual DbSet<SystemMenu> SystemMenus { get; set; }
        public virtual DbSet<timeline> timelines { get; set; }
        public virtual DbSet<attachment_x_event> attachment_x_event { get; set; }
        public virtual DbSet<attachment_x_organization> attachment_x_organization { get; set; }
        public virtual DbSet<attachment_x_product> attachment_x_product { get; set; }
        public virtual DbSet<attachment_x_request> attachment_x_request { get; set; }
        public virtual DbSet<attachment_x_response> attachment_x_response { get; set; }
        public virtual DbSet<organization_x_request> organization_x_request { get; set; }
        public virtual DbSet<rfp_question> rfp_question { get; set; }
    }
}
